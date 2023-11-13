using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class MenuControls
    {
        public int index { get; }
        public string title { get; }
        public string address { get; }
        public List<string> submenus { get; }
        public MenuControls(int index, string title, string address, params string[] submenus)
        {
            this.index = index;
            this.title = title;
            this.address = address;
            this.submenus = new List<string>(submenus);
        }
    }
}
