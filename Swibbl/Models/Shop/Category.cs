namespace Swibbl.Models.Shop
{
    public class Category : BaseModel
    {
        public int count;
        
        private string icon = FontAwesome.FontAwesomeIcons.ChevronDown;

        public string Icon
        {
            get => icon;
            set
            {
                icon = value;
                OnPropertyChanged("Icon");
            }
        }
        public int Count
        {
            get => count;
            set
            {
                count = value;
                OnPropertyChanged("Count");
            }
        }
    }
}
