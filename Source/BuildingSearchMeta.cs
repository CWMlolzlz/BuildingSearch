using ICities;

namespace BuildingSearch {
    public class BuildingSearchMeta : IUserMod {
        public string Name {
            get {
                string output = "Building Search";
                #if DEBUG
                    output += " - debug";
                #endif
                return output;
            }
        }

        public string Description {
            get { return "Quickly search for assets"; }
        }
    }
}