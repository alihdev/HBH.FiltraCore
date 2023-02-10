namespace HBH.FiltraCore
{
    public interface IFiltraCoreRequestInput
    {
        string Filters { get; set; }
        string Term { get; set; }
        string TermBy { get; set; }
    }
}
