namespace Photo_Life_Blazor.Models
{
    public class FilterOption
    {
        public string Filter { get; set; }
        public bool SelectedOption { get; set; }
        public List<string> SelectedFilterString { get; set; }
        public List<int> SelectedFilterInt { get; set; }
        public List<double> SelectedFilterDouble { get; set; }
        public List<DateTime> SelectedFilterDateTime { get; set; }

    }
}
