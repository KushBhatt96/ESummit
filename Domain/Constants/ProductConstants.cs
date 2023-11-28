

namespace Domain.Constants
{
    public class ProductConstants
    {
        public  static IReadOnlyCollection<string> SizeOptions = new HashSet<string>
        {
            "XS", "S", "M", "L", "XL"
        };

        public static IReadOnlyCollection<string> BottomColorOptions = new HashSet<string>
        {
            "black", "whitesmoke", "grey"
        };

        public static IReadOnlyCollection<string> TopColorOptions = new HashSet<string>()
        {
            "black", "whitesmoke", "grey", "green", "lightblue", "pink"
        };

        public static IReadOnlyCollection<string> JacketColorOptions = new HashSet<string>()
        {
            "black", "grey"
        };

        public static IReadOnlyCollection<string> ProductTypes = new HashSet<string>()
        {
            "Tops", "Bottoms", "Jackets"
        };

        public static IReadOnlyCollection<string> Brands = new HashSet<string>()
        {
            "React", "TypeScript", "Redux", "CSharp", "Docker", "Kubernetes"
        };

        public static IReadOnlyCollection<string> Colors = new HashSet<string>()
        {
            "black", "whitesmoke", "grey", "green", "lightblue", "pink"
        };

        public static IReadOnlyCollection<string> Sex = new HashSet<string>()
        {
            "All", "Women", "Men"
        };
    }
}
