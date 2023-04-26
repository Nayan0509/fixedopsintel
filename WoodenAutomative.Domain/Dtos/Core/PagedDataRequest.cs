namespace WoodenAutomative.Domain.Dtos.Core
{
    public abstract class PagedDataRequest
    {
        public int Draw { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }

        public DTSearch Search { get; set; }
        public DTColumn[] Columns { get; set; }
        public DTOrder[] Order { get; set; }

    }

    public class DTColumn
    {
        public string Name { get; set; }
    }

    public class DTOrder
    {
        public int Column { get; set; }

        public DTOrderDir Dir { get; set; }
    }

    public enum DTOrderDir
    {
        asc,
        desc
    }

    public class DTSearch
    {
        public string Value { get; set; }
    }
}
