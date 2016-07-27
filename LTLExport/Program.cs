/* Read a CSV file.
 * Gather Data from user
 * Extract important Col data from import.csv
 * Duplicate Col data
 * Insert User Col data
 * Insert Static Col data
 * Write to output.csv
 * 
 * Copyright (c) 2016 Benjamin Woody
 * 
 *     This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
 * 
*/

using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using System.IO;

namespace LTLExport
{
    class Program
    {
        static void Main(string[] args)
        {
            // Open stream to import
            StreamReader sr = new StreamReader(@"import.csv");

            // Prompt for Data

            Console.WriteLine("What is the Cust code?");
            string cust = Console.ReadLine();
            Console.WriteLine("What is the Carrier Name?");
            string carrier = Console.ReadLine();
            Console.WriteLine("What is the SCAC Code?");
            string SCAC = Console.ReadLine();
            Console.WriteLine("What is the Effective Date (YYYYMMDD)?");
            string effDate = Console.ReadLine();

            // Create a queue of strings(array)
            Queue<string[]> rows = new Queue<string[]>();

            // read each line in the import and add each line to the list.
            while (!sr.EndOfStream)
            {
                string[] Line = sr.ReadLine().Split(',');
                rows.Enqueue(Line);
            }

            //Pop the top (headers)
            var head = rows.Dequeue();

            // Remove blanks from Headers
            head = head.Where(x => !string.IsNullOrEmpty(x)).ToArray();

            // Count the origins
            int origin = head.Count();


            // Count the destinations
            int dest = rows.Count();

            // Open Stream for writing to output.csv
            var outfile = new StreamWriter(File.OpenWrite(@"output.csv"));
            StringBuilder headerOut = new StringBuilder();

            int counter = 1;

            while (rows.Count() != 0)
            {
                var data = rows.Dequeue();
                counter = 1;
                // o keeps track of origin data (col)
                for (int o = 0; o < origin; o++)
                {

                    StringBuilder outline = new StringBuilder();
                    outline.Append(cust);
                    outline.Append(",");
                    outline.Append("********,******");
                    outline.Append(head[o]);
                    outline.Append(",******");
                    outline.Append(data[0]);
                    outline.Append(",");
                    outline.Append(carrier);
                    outline.Append(",G,500,60,");
                    if (data[counter].Contains('%'))
                    {
                        var temp = Convert.ToDecimal(String.Format("{0:0.00}",data[counter].Trim('%')));
                        outline.Append(temp);
                    }
                    else
                        outline.Append(data[counter]);
                    counter++;
                    outline.Append(",,,,,,,");
                    outline.Append(SCAC);
                    outline.Append(",,,");
                    outline.Append(effDate);
                    outline.Append(",UTC,");
                    if (data[counter].Contains('$'))
                    {
                        outline.Append(data[counter].Trim('$'));
                    }
                    else
                        outline.Append(data[counter]);
                    outline.Append(",");
                    if (data[counter].Contains('$'))
                    {
                        outline.Append(data[counter].Trim('$'));
                    }
                    else
                        outline.Append(data[counter]);
                    outline.Append(",");
                    if (data[counter].Contains('$'))
                    {
                        outline.Append(data[counter].Trim('$'));
                    }
                    else
                        outline.Append(data[counter]);
                    counter++;
                    outline.Append(",");
                    outline.Append(",,,$");
                    outline.AppendLine();
                    outfile.Write(outline);

                }

            }
            outfile.Close();
        }

    }

}

