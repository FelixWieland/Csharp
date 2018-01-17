using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labyrinth {
    public static class Visualizer {
        //Generiert aus dem Dungeon Array HTML code;
        public static String toHTML(char[,] dungeon, int value = 40) {
            int width = value;
            int height = value;
            String output = "";
            string folderPath = AppDomain.CurrentDomain.BaseDirectory;
            string style = File.ReadAllText(folderPath + @"\Style.html");
            output += "<!DOCTYPE html><html><head><title>DyDun</title>" + style + "</head><body class=\"dun_tb\">";
            output += "<table style\"width:400px; height: 400px; table-layout:fixed;\">";
            //Durchläuft den Array
            for(int x = 0; x < height; x++) {
                output += "<tr>";
                for (int y = 0; y < width; y++) {
                    output += "<td class=\"" + dungeon[x, y]  +" wh\"></td>";
                }
                output += "</tr>";
            }
            output += "</table>";
            output += "</body></html>";
            return output;
        }
    }
}
