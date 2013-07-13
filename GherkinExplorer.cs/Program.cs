using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using gherkin;

namespace GherkinExplorer.cs
{
    class Program
    {
        static void Main(string[] args)
        {
            var i18N = new I18n("en-ca");
            var keywordTable = i18N.getKeywordTable();
            var codeKeywords = i18N.getCodeKeywords();
            var stepKeywords = i18N.getStepKeywords();

            var stop = true;
        }
    }
}
