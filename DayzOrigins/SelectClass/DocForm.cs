using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Word = Microsoft.Office.Interop.Word;

namespace DayzOrigins
{
    class DocForm
    {
        public static string Path { get; set; }
        private Students Student { get; set; } 

        public DocForm(Students std, string FileName)
        {
            Student = std;
            var WordApp = new Word.Application();
            WordApp.Visible = false;
            var doc = WordApp.Documents.Open(FileName);

            Form("{Data}", "23.05.2014", doc);
            Form("{LastName}", std.LastName, doc);
            Form("{FirstName}", std.FirstName, doc);
            Form("{SecondName}", std.SecondName, doc);
            Form("{DataBerzday}", std.Birth, doc);
            Form("{GroupName}", std.GroupRef.Group.ToString(), doc);
            Form("{SpecName}", std.GroupRef.SpecName, doc);
            doc.SaveAs(Student.FirstName);
            
            //new Window1().richTextBox1.
            //WordApp.Visible = true;
        }

        public static void Form(string Ftext, string Rtext, Word.Document doc)
        {
            var range = doc.Content;

                range.Find.ClearFormatting();
                range.Find.Execute(FindText: Ftext, ReplaceWith: Rtext);
        }
    }
}
