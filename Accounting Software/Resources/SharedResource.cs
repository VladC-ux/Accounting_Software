namespace Accounting_Software.Resources
{
    /// <summary>
    /// Маркер-класс для общих ресурсов локализации.
    /// Лежит в namespace Accounting_Software.Resources, переводы — в
    /// Resources/SharedResource.{culture}.resx рядом.
    /// ВАЖНО: ResourcesPath НЕ задаётся (AddLocalization() без опций), поэтому
    /// фреймворк берёт полное имя типа (Accounting_Software.Resources.SharedResource)
    /// и оно точно совпадает с именем встроенного ресурса. Это обходит проблему
    /// несовпадения имени сборки ("Accounting Software" с пробелом) и RootNamespace.
    /// Используется как IStringLocalizer&lt;SharedResource&gt; / IHtmlLocalizer&lt;SharedResource&gt;.
    /// </summary>
    public class SharedResource
    {
    }
}
