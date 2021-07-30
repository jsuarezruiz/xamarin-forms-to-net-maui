## Migrando um `Custom Renderer` (Xamarin.Forms) para um `Custom Hanlder` (.NET MAUI)

As interfaces de usuário do Xamarin.Forms e .NET MAUI são renderizadas usando controles nativos da plataforma selecionada, permitindo que as aplicações mantenham a aparência e comportamento de cada plataforma. Os `Custom Renderers` do Xamarin.Forms permitem que pessoas desenvolvedoras possam sobrescrever esse processo afim de customizar a aperência e comportamento dos controles de cada plataforma.

Por diversas razões, dentre elas podemos destacar o ganho de performance, mais possibilidades de extenções, temos, no .NET MAUI o conceito de `Custom Handler`. Este conceito é parecido com o do `Custom Renderer`.

Vamos ver passo a passo o processo de migração de um `renderer` para um `handler`! Para focar nos conceitos chaves de `Renderers`e `Handlers` vamos criar um `Entry` customizado, um controle muito familiar. Por outro lado, vamos usar o Android como exemplo, pois tanto a implementação feita para Windows quanto para macOS podem ser acessadas através do código de exemplo.

## Custom Renderer

Todo controle do Xamarin.Forms possui um `renderer` para cada plataforma, este `renderer` cria uma instancia do controle nativo. O processo para a criação de um `renderer` pode ser visto a seguir:

- Criar um controle customizado do Xamarin.Forms;
- Consumir esse controle customizado, através do Xamarin.Forms;
- Criar um `custom renderer` para este controle em cada plataforma.

![Custom Renderer](custom-renderer.png)

### Criando o Entry customizado

Um controle customizado pode ser criado herdando a classe `View`:

```csharp
public class CustomEntry : View
{

}
```

O controle `CustomEntry` é criado no projeto `.NET Standard` e é um simples controle para capturar texto. Para customizar sua aparência e comportamento podemos adicionar `BindableProperties` e `events`.

```csharp
public static readonly BindableProperty TextProperty =
    BindableProperty.Create(nameof(Text), typeof(string), typeof(Entry), string.Empty);


public string Text
{
    get { return (string)GetValue(TextProperty); }
    set { SetValue(TextProperty, value); }
}
```

### Consumindo um controle customizado

O controle `CustomEntry` pode ser referenciado em um arquivo XAML no projeto `.NET Standard`, para isso temos que declarar o `namespace` em que o controle foi implementado e usando um prefixo de `namespace` para o elemento de controle.

```xml
<ContentPage ...
    xmlns:local="clr-namespace:CustomRenderer;assembly=CustomRenderer"
    ...>
    ...
    <local:CustomEntry Text="In Shared Code" />
    ...
</ContentPage>
```

### Criando o Custom Renderer para cada plataforma

O processo de criação da classe do `custom renderer` é:

1. Crie uma classe que herde de **ViewRenderer** que seja o renderer to controle nativo;
2. Sobrescreva o método **OnElementChanged** que renderiza o controle nativo e escreva a lógica necessária para customizar o controle. Esse método é chamado quando o controle Xamarin.Forms correspondente é criado;
3. Sobrescreva o método **OnElementPropertyChanged** que é chamado toda vez que o valor de uma `BindableProperty` muda.
4. Adicione o atributo **ExportRenderer** para a classe do `custom renderer`, para especificar que este `custom renderer` será usado para renderizar o controle Xamarin.Forms. Este atributo é usado para registrar o `custom renderer`.

A classe `ViewRenderer` expõe o método `OnElementChanged`, que é chamado quando o controle Xamarin.Forms é criado para renderizar o controle nativo correspondente. Esse método recebe um **ElementChangedEventArgs** como parâmetro, que contém as propriedades `OldElement` e `NewElement`. Estas propriedades representam os elementos Xamarin.Forms que o `renderer` **estava** atrelado, e o elemento que o `renderer`**está** atrelado, respectivamente. Na aplicação de exemplo a propriedade `OldElement` vai ser nula e a propriedade `NewElement` vai conter a referência para o controle `CustomEntry`.

No método sobrescrito `OnElementChanged`, na classe `CustomEntryRenderer`, é o lugar para executar as customizações do controle nativo. Um referência tipada do controle nativo usada na plataforma pode ser acessada através da propriedade `Control`. É possível, também, acessar a referência do controle Xamarin.Forms através da propriedade `Element`, entretando ela não é usada na aplicação de exemplo.

```csharp
protected override void OnElementChanged(ElementChangedEventArgs<CustomEntry> e)
{
    base.OnElementChanged(e);

    if (e.OldElement == null)
    {
        EditText editText = new EditText(Context);

        _defaultTextColors = editText.TextColors;
        _defaultPlaceholderColors = editText.HintTextColors;

        SetNativeControl(editText);
    }

    UpdateText();
    UpdateTextColor();
    UpdatePlaceholder();
    UpdatePlaceholderColor();
    UpdateCharacterSpacing();
    UpdateHorizontalTextAlignment();
    UpdateVerticalTextAlignment();
}
```

O método sobrescrito `OnElementPropertyChanged` é chamado toda vez que o valor de uma `bindable property` muda, no controle Xamarin.Forms.

```
protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
{
    if (e.PropertyName == CustomEntry.TextProperty.PropertyName)
        UpdateText();
    else if (e.PropertyName == CustomEntry.TextColorProperty.PropertyName)
        UpdateTextColor();
    if (e.PropertyName == CustomEntry.PlaceholderProperty.PropertyName)
        UpdatePlaceholder();
    else if (e.PropertyName == Entry.PlaceholderColorProperty.PropertyName)
        UpdatePlaceholderColor();
    else if (e.PropertyName == Entry.CharacterSpacingProperty.PropertyName)
        UpdateCharacterSpacing();
    else if (e.PropertyName == Entry.HorizontalTextAlignmentProperty.PropertyName)
        UpdateHorizontalTextAlignment();
    else if (e.PropertyName == Entry.VerticalTextAlignmentProperty.PropertyName)
        UpdateVerticalTextAlignment();

    base.OnElementPropertyChanged(sender, e);
}
```

Até o momento nada de novo sob o Sol, apenas uma pequena revisão em como se trabalha com `Custom Renderer` no Xamarin.Forms. Vamos para o .NET MAUI?

## Mas antes, por que essa mudança de Renderers para a arquitetura usada no .NET MAUI?

Para entender as motivações que levaram a essa mudança, mesmo sabendo das implicações e da quantidade de alterações necessárias, precisamos saber o que está errado ou causa confusão no uso dos `renderers`.

_Você lembra do atributo ExportRenderer, usado para registrar o Renderer?_ Ele avisa o Xamarin.Forms, durante a inicialização, fazendo uso de varredura de _assembly_, isto deve procurar em todas as bibliotecas referenciadas o uso desse atributo, e se encontrar, registrar o `renderer`. É fácil de usar, porém ... **varredura de _assembly_ é muito lenta e penaliza a inicialização**.

**O método OnElementChanged comumente causa confusão**. Quando usar o `OldElement`? E o `NewElement`? Quando eu crio meus valores padrões ou faço a assinatura em eventos? E quando eu "desassino" os eventos? Causar confusão é um problema, mas é um problema ainda maior quando se tem um jeito fácil de assinar/"desassinar" cause, as vezes, o cancelamento da assinatura (por exemplo) e, com isso, penalizar a performance.

Todos esses métodos `privados` para atualizar as propriedades no controle nativo são um grande problema. Você pode precisar fazer pequenas mudanças e por conta da falta de acesso (novamente, métodos `privados` aqui e ali!), você acaba criando um `Custom Renderer` maior que o necessário, etc.

Resolvendo esses e outros problemas é o principal objetivo dos `Handlers`.

## Custom Handlers

O processo para criação de um `handler` customizado pode ser visto a seguir:

1. Criar uma classe que herde de **ViewHandler** que renderiza o controle nativo;
2. Sobrescrever o método **CreateNativeView** que renderiza o controle nativo;
3. Criar um dicionário **Mapper** que responda as mudanças de valores das propriedades;
4. Registrar o `handler` usando o método **AddHandler** na classe `Startup`.

![Custom Handler](custom-handler.png)

### Criando o Entry customizado

`Handlers` podem ser acessados através da interface de controle específico fornecida e derivada da interface **IView**. Isso evita que o controle multi-plataforma tenha que referenciar seu `handler`, e que o `handler` tenha uma referência para o controle multi-plataforma. O mapeamento da API do controle multi-plataforma para a API da plataforma é fornecida por um `mapper`.

Deste modo começamos criando a interface que irá definir nosso controle:

```csharp
public interface ICustomEntry : IView
{
    public string Text { get; }
    public Color TextColor { get; }
    public string Placeholder { get; }
    public Color PlaceholderColor { get; }
    public double CharacterSpacing { get; }
    public TextAlignment HorizontalTextAlignment { get; }
    public TextAlignment VerticalTextAlignment { get; }

    void Completed();
}
```

O controle customizado pode ser criado através de uma classe que herde de `View` e implemente a interface do controle, criada anteriormente.

```csharp
public class CustomEntry : View, ICustomEntry
{

}
```

### Criando o Custom Handler para cada plataforma

Crie uma classe que herde de **ViewHandler** que renderize o controle nativo.

```csharp
public partial class CustomEntryHandler : ViewHandler<ICustomEntry, EditText>
{

}
```

Isso parece uma mudança banal, nós mudamos a classe base de `ViewRenderer` para `ViewHandler`, porém tem muito mais!

A classe `ViewRenderer` do Xamarin.Forms cria um elemento pai, no caso do Android é um `ViewGroup`, que era usado para auxiliar nas tarefas de posicionamento. **ViewHandler não cria nenhum elemento pai** o que ajuda a reduzir o número de elementos visuais, o que melhora a performance.

Ao herdar de `ViewHandler`, temos que implementar o método **CreateNativeView**.

```csharp
protected override EditText CreateNativeView()
{
    return new EditText(Context);
}
```

Anteriormente revisamos como o método `OnElementChanged` era usado no Xamarin.Forms, lembra? Neste método criamos o controle nativo, inicializamos os valores padrões e assinamos eventos, etc. Entretanto, esse modelo demandava uma grande quantidade de conhecimento, por exemplo, saber qual era o `OldElement` e `NewElement`.

O .NET MAUI simplificou e refatorou tudo o que fazíamos no método `OnElementChanged` em diferentes métodos, de um jeito bem simples.

Criamos o controle nativo no método `CreateNativeView`. Por outro lado, temos outros métodos como **ConnectHandler** e **DisconnectHandler**.

```csharp
protected override void ConnectHandler(EditText nativeView)
{
    _defaultTextColors = nativeView.TextColors;
    _defaultPlaceholderColors = nativeView.HintTextColors;

    _watcher.Handler = this;
    nativeView.AddTextChangedListener(_watcher);

    base.ConnectHandler(nativeView);
}

protected override void DisconnectHandler(EditText nativeView)
{
    nativeView.RemoveTextChangedListener(_watcher);
    _watcher.Handler = null;

    base.DisconnectHandler(nativeView);
}
```

O método `ConnectHandler` é o lugar ideal para fazer inicializar propriedades, assinar eventos, etc. E, seguindo a mesma ideia, podemos fazer o `dispose`, "desassinar" eventos, etc no método `DisconnectHandler`.

### Mapper

O **mapper** é um novo conceito introduzido pelos `Handlers`. É apenas um dicionário com propriedades e `Actions` definidas na interface do controle (lembra-se, nós usamos interfaces nos `Handlers`). Isso substitui o que era feito pelo método `OnElementPropertyChanged` no Xamarin.Forms.

```csharp
public static PropertyMapper<ICustomEntry, CustomEntryHandler> CustomEntryMapper = new PropertyMapper<ICustomEntry, CustomEntryHandler>(ViewHandler.ViewMapper)
{
    [nameof(ICustomEntry.Text)] = MapText,
    [nameof(ICustomEntry.TextColor)] = MapTextColor,
    [nameof(ICustomEntry.Placeholder)] = MapPlaceholder,
    [nameof(ICustomEntry.PlaceholderColor)] = MapPlaceholderColor,
    [nameof(ICustomEntry.CharacterSpacing)] = MapCharacterSpacing,
    [nameof(ICustomEntry.HorizontalLayoutAlignment)] = MapHorizontalLayoutAlignment,
    [nameof(ICustomEntry.VerticalLayoutAlignment)] = MapVerticalLayoutAlignment
};
```

O `Mapper` atrela as propriedades à métodos estáticos.

```csharp
public static void MapText(CustomEntryHandler handler, ICustomEntry entry)
{
    handler.NativeView?.UpdateText(entry);
}
```

O `Mapper` simplifica o gerenciamento das mudanças de valores das propriedades (notifica as propriedades durante a inicialização do controle e também, toda vez que o valor de uma propriedade mudar) nos permite mais opções de custmoizações.

Por exemplo:

```csharp
using Microsoft.Maui;
using Microsoft.Maui.Controls;


public partial class App : Application
{
    public App()
    {
        InitializeComponent();

#if __ANDROID__
        CustomEntryMapper[nameof(ICustomEntry.Text)] = (handler, view) =>
        {
            (handler.NativeView as Android.Widget.EditText).Text = view.text + "custom";
        };
#endif
    }
}
```

### Registrando um Handler

Diferentemente do Xamarin.Forms, que utilizava o atributo `ExportRenderer`, que forçava uma varredura de _assembly_, no .NET MAUI o registro do `handler` é um pouco diferente.

```csharp
appBuilder
    .UseMauiApp<App>()
    .ConfigureMauiHandlers(handlers =>
    {
#if __ANDROID__
        handlers.AddHandler(typeof(CustomEntry), typeof(CustomEntryHandler));
#endif
    });
```

Fazemos uso do `AppHostBuilder`e do método **AddHandler** para registrar o `Handler`. No fim das contas, isso requer, como no Xamarin.Forms, uma linha para indicar que queremos registrar um `Handler`, mas o uso da varredura de _assembly_ é evitado, que é lento e caro, penalizando o tempo de inicialização do aplicativo.
