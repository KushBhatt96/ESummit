using System.Collections.ObjectModel;

namespace Domain.Constants
{
    public class AddressConstants
    {
        public ReadOnlyCollection<string> Countries => new(new List<string>
        {
            "Canada", "USA"
        });

        public ReadOnlyCollection<string> CanadianProvinces => new(new List<string>
        {
            "AB", "BC", "ON", "QC"
        });

        public ReadOnlyCollection<string> AmericanStates => new(new List<string>
        {
            "CA", "NY", "TX", "WA"
        });
    }
}
