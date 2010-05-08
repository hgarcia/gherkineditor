using System.Collections.Generic;

namespace GherkinEditor
{
    public class Languages : List<Language>
    {
        public Languages()
        {
            FillUpList();
        }

        private void FillUpList()
        {
        	
            Add(new Language("ar","Arabic","العربية"));
            
            Add(new Language("bg","Bulgarian","български"));
            
            Add(new Language("ca","Catalan","català"));
            
            Add(new Language("cs","Czech","Česky"));
            
            Add(new Language("cy-GB","Welsh","Cymraeg"));
            
            Add(new Language("da","Danish","dansk"));
            
            Add(new Language("de","German","Deutsch"));
            
            Add(new Language("en","English","English"));
            
            Add(new Language("en-Scouse","Scouse","Scouse"));
            
            Add(new Language("en-au","Australian","Australian"));
            
            Add(new Language("en-lol","LOLCAT","LOLCAT"));
            
            Add(new Language("en-tx","Texan","Texan"));
            
            Add(new Language("eo","Esperanto","Esperanto"));
            
            Add(new Language("es","Spanish","español"));
            
            Add(new Language("et","Estonian","eesti keel"));
            
            Add(new Language("fi","Finnish","suomi"));
            
            Add(new Language("fr","French","français"));
            
            Add(new Language("he","Hebrew","עברית"));
            
            Add(new Language("hr","Croatian","hrvatski"));
            
            Add(new Language("hu","Hungarian","magyar"));
            
            Add(new Language("id","Indonesian","Bahasa Indonesia"));
            
            Add(new Language("it","Italian","italiano"));
            
            Add(new Language("ja","Japanese","日本語"));
            
            Add(new Language("ko","Korean","한국어"));
            
            Add(new Language("lt","Lithuanian","lietuvių kalba"));
            
            Add(new Language("lv","Latvian","latviešu"));
            
            Add(new Language("nl","Dutch","Nederlands"));
            
            Add(new Language("no","Norwegian","norsk"));
            
            Add(new Language("pl","Polish","polski"));
            
            Add(new Language("pt","Portuguese","português"));
            
            Add(new Language("ro","Romanian","română"));
            
            Add(new Language("ro-RO","Romanian (diacritical)","română (diacritical)"));
            
            Add(new Language("ru","Russian","русский"));
            
            Add(new Language("sk","Slovak","Slovensky"));
            
            Add(new Language("sr-Cyrl","Serbian","Српски"));
            
            Add(new Language("sr-Latn","Serbian (Latin)","Srpski (Latinica)"));
            
            Add(new Language("sv","Swedish","Svenska"));
            
            Add(new Language("tr","Turkish","Türkçe"));
            
            Add(new Language("uk","Ukrainian","Українська"));
            
            Add(new Language("uz","Uzbek","Узбекча"));
            
            Add(new Language("vi","Vietnamese","Tiếng Việt"));
            
            Add(new Language("zh-CN","Chinese simplified","简体中文"));
            
            Add(new Language("zh-TW","Chinese traditional","繁體中文"));
            
        }

        public Language GetByIso(string isoCode)
        {
            return Find(l => l.IsoCode == isoCode) ?? new Language("en", "English", "English");
        }        
    }
}