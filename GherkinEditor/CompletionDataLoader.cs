using System;
using System.Collections.Generic;
using ICSharpCode.AvalonEdit.CodeCompletion;
using System.Linq;

namespace GherkinEditor
{
    public class CompletionDataLoader
    {
        private readonly Languages _langs;
        private IDictionary<Language, IList<ICompletionData>> _completionDataByLanguage;

        public CompletionDataLoader()
        {
            _langs =  new Languages();
            LoadCompletionData();
        }

        private void LoadCompletionData()
        {
        	_completionDataByLanguage =  new Dictionary<Language, IList<ICompletionData>>();
        	
            _completionDataByLanguage.Add(_langs.GetByIso("ar"),new List<ICompletionData>
                                                        {
                                                        	
                                                            new GherkinCompletionItem("سيناريو"),
                                                            new GherkinCompletionItem("خاصية"),
                                                            new GherkinCompletionItem("متى "),
                                                            new GherkinCompletionItem("اذاً "),
                                                            new GherkinCompletionItem("بفرض "),
                                                            new GherkinCompletionItem("و "),
                                                            new GherkinCompletionItem("لكن "),
                                                            new GherkinCompletionItem("سيناريو مخطط"),
                                                            new GherkinCompletionItem("امثلة"),
                                                            new GherkinCompletionItem("الخلفية")                                                            
                                                        });
            
            _completionDataByLanguage.Add(_langs.GetByIso("bg"),new List<ICompletionData>
                                                        {
                                                        	
                                                            new GherkinCompletionItem("Сценарий"),
                                                            new GherkinCompletionItem("Функционалност"),
                                                            new GherkinCompletionItem("Когато "),
                                                            new GherkinCompletionItem("То "),
                                                            new GherkinCompletionItem("Дадено "),
                                                            new GherkinCompletionItem("И "),
                                                            new GherkinCompletionItem("Но "),
                                                            new GherkinCompletionItem("Рамка на сценарий"),
                                                            new GherkinCompletionItem("Примери"),
                                                            new GherkinCompletionItem("Предистория")                                                            
                                                        });
            
            _completionDataByLanguage.Add(_langs.GetByIso("ca"),new List<ICompletionData>
                                                        {
                                                        	
                                                            new GherkinCompletionItem("Escenari"),
                                                            new GherkinCompletionItem("CaracterísticaFuncionalitat"),
                                                            new GherkinCompletionItem("Quan "),
                                                            new GherkinCompletionItem("Aleshores "),
                                                            new GherkinCompletionItem("Donat "),
                                                            new GherkinCompletionItem("I "),
                                                            new GherkinCompletionItem("Però "),
                                                            new GherkinCompletionItem("Esquema de l'escenari"),
                                                            new GherkinCompletionItem("Exemples"),
                                                            new GherkinCompletionItem("RerefonsAntecedents")                                                            
                                                        });
            
            _completionDataByLanguage.Add(_langs.GetByIso("cs"),new List<ICompletionData>
                                                        {
                                                        	
                                                            new GherkinCompletionItem("Scénář"),
                                                            new GherkinCompletionItem("Požadavek"),
                                                            new GherkinCompletionItem("Když "),
                                                            new GherkinCompletionItem("Pak "),
                                                            new GherkinCompletionItem("Pokud "),
                                                            new GherkinCompletionItem("A "),
                                                            new GherkinCompletionItem("Ale "),
                                                            new GherkinCompletionItem("Náčrt ScénářeOsnova scénáře"),
                                                            new GherkinCompletionItem("Příklady"),
                                                            new GherkinCompletionItem("PozadíKontext")                                                            
                                                        });
            
            _completionDataByLanguage.Add(_langs.GetByIso("cy-GB"),new List<ICompletionData>
                                                        {
                                                        	
                                                            new GherkinCompletionItem("Scenario"),
                                                            new GherkinCompletionItem("Arwedd"),
                                                            new GherkinCompletionItem("Pryd "),
                                                            new GherkinCompletionItem("Yna "),
                                                            new GherkinCompletionItem("Anrhegedig a "),
                                                            new GherkinCompletionItem("A "),
                                                            new GherkinCompletionItem("Ond "),
                                                            new GherkinCompletionItem("Scenario Amlinellol"),
                                                            new GherkinCompletionItem("Enghreifftiau"),
                                                            new GherkinCompletionItem("Cefndir")                                                            
                                                        });
            
            _completionDataByLanguage.Add(_langs.GetByIso("da"),new List<ICompletionData>
                                                        {
                                                        	
                                                            new GherkinCompletionItem("Scenarie"),
                                                            new GherkinCompletionItem("Egenskab"),
                                                            new GherkinCompletionItem("Når "),
                                                            new GherkinCompletionItem("Så "),
                                                            new GherkinCompletionItem("Givet "),
                                                            new GherkinCompletionItem("Og "),
                                                            new GherkinCompletionItem("Men "),
                                                            new GherkinCompletionItem("Abstrakt Scenario"),
                                                            new GherkinCompletionItem("Eksempler"),
                                                            new GherkinCompletionItem("Baggrund")                                                            
                                                        });
            
            _completionDataByLanguage.Add(_langs.GetByIso("de"),new List<ICompletionData>
                                                        {
                                                        	
                                                            new GherkinCompletionItem("Szenario"),
                                                            new GherkinCompletionItem("Funktionalität"),
                                                            new GherkinCompletionItem("Wenn "),
                                                            new GherkinCompletionItem("Dann "),
                                                            new GherkinCompletionItem("Angenommen "),
                                                            new GherkinCompletionItem("Und "),
                                                            new GherkinCompletionItem("Aber "),
                                                            new GherkinCompletionItem("Szenariogrundriss"),
                                                            new GherkinCompletionItem("Beispiele"),
                                                            new GherkinCompletionItem("Grundlage")                                                            
                                                        });
            
            _completionDataByLanguage.Add(_langs.GetByIso("en"),new List<ICompletionData>
                                                        {
                                                        	
                                                            new GherkinCompletionItem("Scenario"),
                                                            new GherkinCompletionItem("Feature"),
                                                            new GherkinCompletionItem("When "),
                                                            new GherkinCompletionItem("Then "),
                                                            new GherkinCompletionItem("Given "),
                                                            new GherkinCompletionItem("And "),
                                                            new GherkinCompletionItem("But "),
                                                            new GherkinCompletionItem("Scenario Outline"),
                                                            new GherkinCompletionItem("Examples"),
                                                            new GherkinCompletionItem("Background")                                                            
                                                        });
            
            _completionDataByLanguage.Add(_langs.GetByIso("en-Scouse"),new List<ICompletionData>
                                                        {
                                                        	
                                                            new GherkinCompletionItem("The thing of it is"),
                                                            new GherkinCompletionItem("Feature"),
                                                            new GherkinCompletionItem("Wun "),
                                                            new GherkinCompletionItem("Dun "),
                                                            new GherkinCompletionItem("Givun "),
                                                            new GherkinCompletionItem("An "),
                                                            new GherkinCompletionItem("Buh "),
                                                            new GherkinCompletionItem("Wharrimean is"),
                                                            new GherkinCompletionItem("Examples"),
                                                            new GherkinCompletionItem("Dis is what went down")                                                            
                                                        });
            
            _completionDataByLanguage.Add(_langs.GetByIso("en-au"),new List<ICompletionData>
                                                        {
                                                        	
                                                            new GherkinCompletionItem("Mate"),
                                                            new GherkinCompletionItem("Crikey"),
                                                            new GherkinCompletionItem("When "),
                                                            new GherkinCompletionItem("Ya gotta "),
                                                            new GherkinCompletionItem("Ya know how "),
                                                            new GherkinCompletionItem("N "),
                                                            new GherkinCompletionItem("Cept "),
                                                            new GherkinCompletionItem("Blokes"),
                                                            new GherkinCompletionItem("Cobber"),
                                                            new GherkinCompletionItem("Background")                                                            
                                                        });
            
            _completionDataByLanguage.Add(_langs.GetByIso("en-lol"),new List<ICompletionData>
                                                        {
                                                        	
                                                            new GherkinCompletionItem("MISHUN"),
                                                            new GherkinCompletionItem("OH HAI"),
                                                            new GherkinCompletionItem("WEN "),
                                                            new GherkinCompletionItem("DEN "),
                                                            new GherkinCompletionItem("I CAN HAZ "),
                                                            new GherkinCompletionItem("AN "),
                                                            new GherkinCompletionItem("BUT "),
                                                            new GherkinCompletionItem("MISHUN SRSLY"),
                                                            new GherkinCompletionItem("EXAMPLZ"),
                                                            new GherkinCompletionItem("B4")                                                            
                                                        });
            
            _completionDataByLanguage.Add(_langs.GetByIso("en-tx"),new List<ICompletionData>
                                                        {
                                                        	
                                                            new GherkinCompletionItem("Scenario"),
                                                            new GherkinCompletionItem("Feature"),
                                                            new GherkinCompletionItem("When y'all "),
                                                            new GherkinCompletionItem("Then y'all "),
                                                            new GherkinCompletionItem("Given y'all "),
                                                            new GherkinCompletionItem("And y'all "),
                                                            new GherkinCompletionItem("But y'all "),
                                                            new GherkinCompletionItem("All y'all"),
                                                            new GherkinCompletionItem("Examples"),
                                                            new GherkinCompletionItem("Background")                                                            
                                                        });
            
            _completionDataByLanguage.Add(_langs.GetByIso("eo"),new List<ICompletionData>
                                                        {
                                                        	
                                                            new GherkinCompletionItem("Scenaro"),
                                                            new GherkinCompletionItem("Trajto"),
                                                            new GherkinCompletionItem("Se "),
                                                            new GherkinCompletionItem("Do "),
                                                            new GherkinCompletionItem("Donitaĵo "),
                                                            new GherkinCompletionItem("Kaj "),
                                                            new GherkinCompletionItem("Sed "),
                                                            new GherkinCompletionItem("Konturo de la scenaro"),
                                                            new GherkinCompletionItem("Ekzemploj"),
                                                            new GherkinCompletionItem("Fono")                                                            
                                                        });
            
            _completionDataByLanguage.Add(_langs.GetByIso("es"),new List<ICompletionData>
                                                        {
                                                        	
                                                            new GherkinCompletionItem("Escenario"),
                                                            new GherkinCompletionItem("Característica"),
                                                            new GherkinCompletionItem("Cuando "),
                                                            new GherkinCompletionItem("Entonces "),
                                                            new GherkinCompletionItem("Dado "),
                                                            new GherkinCompletionItem("Y "),
                                                            new GherkinCompletionItem("Pero "),
                                                            new GherkinCompletionItem("Esquema del escenario"),
                                                            new GherkinCompletionItem("Ejemplos"),
                                                            new GherkinCompletionItem("Antecedentes")                                                            
                                                        });
            
            _completionDataByLanguage.Add(_langs.GetByIso("et"),new List<ICompletionData>
                                                        {
                                                        	
                                                            new GherkinCompletionItem("Stsenaarium"),
                                                            new GherkinCompletionItem("Omadus"),
                                                            new GherkinCompletionItem("Kui "),
                                                            new GherkinCompletionItem("Siis "),
                                                            new GherkinCompletionItem("Eeldades "),
                                                            new GherkinCompletionItem("Ja "),
                                                            new GherkinCompletionItem("Kuid "),
                                                            new GherkinCompletionItem("Raamstsenaarium"),
                                                            new GherkinCompletionItem("Juhtumid"),
                                                            new GherkinCompletionItem("Taust")                                                            
                                                        });
            
            _completionDataByLanguage.Add(_langs.GetByIso("fi"),new List<ICompletionData>
                                                        {
                                                        	
                                                            new GherkinCompletionItem("Tapaus"),
                                                            new GherkinCompletionItem("Ominaisuus"),
                                                            new GherkinCompletionItem("Kun "),
                                                            new GherkinCompletionItem("Niin "),
                                                            new GherkinCompletionItem("Oletetaan "),
                                                            new GherkinCompletionItem("Ja "),
                                                            new GherkinCompletionItem("Mutta "),
                                                            new GherkinCompletionItem("Tapausaihio"),
                                                            new GherkinCompletionItem("Tapaukset"),
                                                            new GherkinCompletionItem("Tausta")                                                            
                                                        });
            
            _completionDataByLanguage.Add(_langs.GetByIso("fr"),new List<ICompletionData>
                                                        {
                                                        	
                                                            new GherkinCompletionItem("Scénario"),
                                                            new GherkinCompletionItem("Fonctionnalité"),
                                                            new GherkinCompletionItem("Quand "),
                                                            new GherkinCompletionItem("Alors "),
                                                            new GherkinCompletionItem("Soit "),
                                                            new GherkinCompletionItem("Et "),
                                                            new GherkinCompletionItem("Mais "),
                                                            new GherkinCompletionItem("Plan du scénarioPlan du Scénario"),
                                                            new GherkinCompletionItem("Exemples"),
                                                            new GherkinCompletionItem("Contexte")                                                            
                                                        });
            
            _completionDataByLanguage.Add(_langs.GetByIso("he"),new List<ICompletionData>
                                                        {
                                                        	
                                                            new GherkinCompletionItem("תרחיש"),
                                                            new GherkinCompletionItem("תכונה"),
                                                            new GherkinCompletionItem("כאשר "),
                                                            new GherkinCompletionItem("אז "),
                                                            new GherkinCompletionItem("בהינתן "),
                                                            new GherkinCompletionItem("וגם "),
                                                            new GherkinCompletionItem("אבל "),
                                                            new GherkinCompletionItem("תבנית תרחיש"),
                                                            new GherkinCompletionItem("דוגמאות"),
                                                            new GherkinCompletionItem("רקע")                                                            
                                                        });
            
            _completionDataByLanguage.Add(_langs.GetByIso("hr"),new List<ICompletionData>
                                                        {
                                                        	
                                                            new GherkinCompletionItem("Scenarij"),
                                                            new GherkinCompletionItem("OsobinaMogućnostMogucnost"),
                                                            new GherkinCompletionItem("Kada "),
                                                            new GherkinCompletionItem("Onda "),
                                                            new GherkinCompletionItem("Zadan "),
                                                            new GherkinCompletionItem("I "),
                                                            new GherkinCompletionItem("Ali "),
                                                            new GherkinCompletionItem("SkicaKoncept"),
                                                            new GherkinCompletionItem("Primjeri"),
                                                            new GherkinCompletionItem("Pozadina")                                                            
                                                        });
            
            _completionDataByLanguage.Add(_langs.GetByIso("hu"),new List<ICompletionData>
                                                        {
                                                        	
                                                            new GherkinCompletionItem("Forgatókönyv"),
                                                            new GherkinCompletionItem("Jellemző"),
                                                            new GherkinCompletionItem("Majd "),
                                                            new GherkinCompletionItem("Akkor "),
                                                            new GherkinCompletionItem("Ha "),
                                                            new GherkinCompletionItem("És "),
                                                            new GherkinCompletionItem("De "),
                                                            new GherkinCompletionItem("Forgatókönyv vázlat"),
                                                            new GherkinCompletionItem("Példák"),
                                                            new GherkinCompletionItem("Háttér")                                                            
                                                        });
            
            _completionDataByLanguage.Add(_langs.GetByIso("id"),new List<ICompletionData>
                                                        {
                                                        	
                                                            new GherkinCompletionItem("Skenario"),
                                                            new GherkinCompletionItem("Fitur"),
                                                            new GherkinCompletionItem("Ketika "),
                                                            new GherkinCompletionItem("Maka "),
                                                            new GherkinCompletionItem("Dengan "),
                                                            new GherkinCompletionItem("Dan "),
                                                            new GherkinCompletionItem("Tapi "),
                                                            new GherkinCompletionItem("Skenario konsep"),
                                                            new GherkinCompletionItem("Contoh"),
                                                            new GherkinCompletionItem("Dasar")                                                            
                                                        });
            
            _completionDataByLanguage.Add(_langs.GetByIso("it"),new List<ICompletionData>
                                                        {
                                                        	
                                                            new GherkinCompletionItem("Scenario"),
                                                            new GherkinCompletionItem("Funzionalità"),
                                                            new GherkinCompletionItem("Quando "),
                                                            new GherkinCompletionItem("Allora "),
                                                            new GherkinCompletionItem("Dato "),
                                                            new GherkinCompletionItem("E "),
                                                            new GherkinCompletionItem("Ma "),
                                                            new GherkinCompletionItem("Schema dello scenario"),
                                                            new GherkinCompletionItem("Esempi"),
                                                            new GherkinCompletionItem("Contesto")                                                            
                                                        });
            
            _completionDataByLanguage.Add(_langs.GetByIso("ja"),new List<ICompletionData>
                                                        {
                                                        	
                                                            new GherkinCompletionItem("シナリオ"),
                                                            new GherkinCompletionItem("フィーチャ機能"),
                                                            new GherkinCompletionItem("もし"),
                                                            new GherkinCompletionItem("ならば"),
                                                            new GherkinCompletionItem("前提"),
                                                            new GherkinCompletionItem("かつ"),
                                                            new GherkinCompletionItem("しかし"),
                                                            new GherkinCompletionItem("シナリオアウトラインシナリオテンプレートテンプレシナリオテンプレ"),
                                                            new GherkinCompletionItem("例"),
                                                            new GherkinCompletionItem("背景")                                                            
                                                        });
            
            _completionDataByLanguage.Add(_langs.GetByIso("ko"),new List<ICompletionData>
                                                        {
                                                        	
                                                            new GherkinCompletionItem("시나리오"),
                                                            new GherkinCompletionItem("기능"),
                                                            new GherkinCompletionItem("만일"),
                                                            new GherkinCompletionItem("그러면"),
                                                            new GherkinCompletionItem("조건"),
                                                            new GherkinCompletionItem("그리고"),
                                                            new GherkinCompletionItem("하지만"),
                                                            new GherkinCompletionItem("시나리오 개요"),
                                                            new GherkinCompletionItem("예"),
                                                            new GherkinCompletionItem("배경")                                                            
                                                        });
            
            _completionDataByLanguage.Add(_langs.GetByIso("lt"),new List<ICompletionData>
                                                        {
                                                        	
                                                            new GherkinCompletionItem("Scenarijus"),
                                                            new GherkinCompletionItem("Savybė"),
                                                            new GherkinCompletionItem("Kai "),
                                                            new GherkinCompletionItem("Tada "),
                                                            new GherkinCompletionItem("Duota "),
                                                            new GherkinCompletionItem("Ir "),
                                                            new GherkinCompletionItem("Bet "),
                                                            new GherkinCompletionItem("Scenarijaus šablonas"),
                                                            new GherkinCompletionItem("Pavyzdžiai"),
                                                            new GherkinCompletionItem("Kontekstas")                                                            
                                                        });
            
            _completionDataByLanguage.Add(_langs.GetByIso("lv"),new List<ICompletionData>
                                                        {
                                                        	
                                                            new GherkinCompletionItem("Scenārijs"),
                                                            new GherkinCompletionItem("FunkcionalitāteFīča"),
                                                            new GherkinCompletionItem("Ja "),
                                                            new GherkinCompletionItem("Tad "),
                                                            new GherkinCompletionItem("Kad "),
                                                            new GherkinCompletionItem("Un "),
                                                            new GherkinCompletionItem("Bet "),
                                                            new GherkinCompletionItem("Scenārijs pēc parauga"),
                                                            new GherkinCompletionItem("Piemēri"),
                                                            new GherkinCompletionItem("KontekstsSituācija")                                                            
                                                        });
            
            _completionDataByLanguage.Add(_langs.GetByIso("nl"),new List<ICompletionData>
                                                        {
                                                        	
                                                            new GherkinCompletionItem("Scenario"),
                                                            new GherkinCompletionItem("Functionaliteit"),
                                                            new GherkinCompletionItem("Als "),
                                                            new GherkinCompletionItem("Dan "),
                                                            new GherkinCompletionItem("Gegeven "),
                                                            new GherkinCompletionItem("En "),
                                                            new GherkinCompletionItem("Maar "),
                                                            new GherkinCompletionItem("Abstract Scenario"),
                                                            new GherkinCompletionItem("Voorbeelden"),
                                                            new GherkinCompletionItem("Achtergrond")                                                            
                                                        });
            
            _completionDataByLanguage.Add(_langs.GetByIso("no"),new List<ICompletionData>
                                                        {
                                                        	
                                                            new GherkinCompletionItem("Scenario"),
                                                            new GherkinCompletionItem("Egenskap"),
                                                            new GherkinCompletionItem("Når "),
                                                            new GherkinCompletionItem("Så "),
                                                            new GherkinCompletionItem("Gitt "),
                                                            new GherkinCompletionItem("Og "),
                                                            new GherkinCompletionItem("Men "),
                                                            new GherkinCompletionItem("Abstrakt Scenario"),
                                                            new GherkinCompletionItem("Eksempler"),
                                                            new GherkinCompletionItem("Bakgrunn")                                                            
                                                        });
            
            _completionDataByLanguage.Add(_langs.GetByIso("pl"),new List<ICompletionData>
                                                        {
                                                        	
                                                            new GherkinCompletionItem("Scenariusz"),
                                                            new GherkinCompletionItem("Właściwość"),
                                                            new GherkinCompletionItem("Jeżeli "),
                                                            new GherkinCompletionItem("Wtedy "),
                                                            new GherkinCompletionItem("Zakładając "),
                                                            new GherkinCompletionItem("Oraz "),
                                                            new GherkinCompletionItem("Ale "),
                                                            new GherkinCompletionItem("Szablon scenariusza"),
                                                            new GherkinCompletionItem("Przykłady"),
                                                            new GherkinCompletionItem("Założenia")                                                            
                                                        });
            
            _completionDataByLanguage.Add(_langs.GetByIso("pt"),new List<ICompletionData>
                                                        {
                                                        	
                                                            new GherkinCompletionItem("CenárioCenario"),
                                                            new GherkinCompletionItem("Funcionalidade"),
                                                            new GherkinCompletionItem("Quando "),
                                                            new GherkinCompletionItem("Então "),
                                                            new GherkinCompletionItem("Dado "),
                                                            new GherkinCompletionItem("E "),
                                                            new GherkinCompletionItem("Mas "),
                                                            new GherkinCompletionItem("Esquema do CenárioEsquema do Cenario"),
                                                            new GherkinCompletionItem("Exemplos"),
                                                            new GherkinCompletionItem("Contexto")                                                            
                                                        });
            
            _completionDataByLanguage.Add(_langs.GetByIso("ro"),new List<ICompletionData>
                                                        {
                                                        	
                                                            new GherkinCompletionItem("Scenariu"),
                                                            new GherkinCompletionItem("Functionalitate"),
                                                            new GherkinCompletionItem("Cand "),
                                                            new GherkinCompletionItem("Atunci "),
                                                            new GherkinCompletionItem("Daca "),
                                                            new GherkinCompletionItem("Si "),
                                                            new GherkinCompletionItem("Dar "),
                                                            new GherkinCompletionItem("Scenariul de sablon"),
                                                            new GherkinCompletionItem("Exemplele"),
                                                            new GherkinCompletionItem("Conditii")                                                            
                                                        });
            
            _completionDataByLanguage.Add(_langs.GetByIso("ro-RO"),new List<ICompletionData>
                                                        {
                                                        	
                                                            new GherkinCompletionItem("Scenariu"),
                                                            new GherkinCompletionItem("Funcționalitate"),
                                                            new GherkinCompletionItem("Când "),
                                                            new GherkinCompletionItem("Atunci "),
                                                            new GherkinCompletionItem("Dacă "),
                                                            new GherkinCompletionItem("Și "),
                                                            new GherkinCompletionItem("Dar "),
                                                            new GherkinCompletionItem("Scenariul de şablon"),
                                                            new GherkinCompletionItem("Exemplele"),
                                                            new GherkinCompletionItem("Condiţii")                                                            
                                                        });
            
            _completionDataByLanguage.Add(_langs.GetByIso("ru"),new List<ICompletionData>
                                                        {
                                                        	
                                                            new GherkinCompletionItem("Сценарий"),
                                                            new GherkinCompletionItem("ФункционалФича"),
                                                            new GherkinCompletionItem("Если "),
                                                            new GherkinCompletionItem("То "),
                                                            new GherkinCompletionItem("Допустим "),
                                                            new GherkinCompletionItem("И "),
                                                            new GherkinCompletionItem("Но "),
                                                            new GherkinCompletionItem("Структура сценария"),
                                                            new GherkinCompletionItem("Значения"),
                                                            new GherkinCompletionItem("Предыстория")                                                            
                                                        });
            
            _completionDataByLanguage.Add(_langs.GetByIso("sk"),new List<ICompletionData>
                                                        {
                                                        	
                                                            new GherkinCompletionItem("Scenár"),
                                                            new GherkinCompletionItem("Požiadavka"),
                                                            new GherkinCompletionItem("Keď "),
                                                            new GherkinCompletionItem("Tak "),
                                                            new GherkinCompletionItem("Pokiaľ "),
                                                            new GherkinCompletionItem("A "),
                                                            new GherkinCompletionItem("Ale "),
                                                            new GherkinCompletionItem("Náčrt Scenáru"),
                                                            new GherkinCompletionItem("Príklady"),
                                                            new GherkinCompletionItem("Pozadie")                                                            
                                                        });
            
            _completionDataByLanguage.Add(_langs.GetByIso("sr-Cyrl"),new List<ICompletionData>
                                                        {
                                                        	
                                                            new GherkinCompletionItem("СценариоПример"),
                                                            new GherkinCompletionItem("ФункционалностМогућностОсобина"),
                                                            new GherkinCompletionItem("Када "),
                                                            new GherkinCompletionItem("Онда "),
                                                            new GherkinCompletionItem("Задато "),
                                                            new GherkinCompletionItem("И "),
                                                            new GherkinCompletionItem("Али "),
                                                            new GherkinCompletionItem("Структура сценаријаСкицаКонцепт"),
                                                            new GherkinCompletionItem("Примери"),
                                                            new GherkinCompletionItem("КонтекстОсноваПозадина")                                                            
                                                        });
            
            _completionDataByLanguage.Add(_langs.GetByIso("sr-Latn"),new List<ICompletionData>
                                                        {
                                                        	
                                                            new GherkinCompletionItem("ScenarioPrimer"),
                                                            new GherkinCompletionItem("FunkcionalnostMogućnostMogucnostOsobina"),
                                                            new GherkinCompletionItem("Kada "),
                                                            new GherkinCompletionItem("Onda "),
                                                            new GherkinCompletionItem("Zadato "),
                                                            new GherkinCompletionItem("I "),
                                                            new GherkinCompletionItem("Ali "),
                                                            new GherkinCompletionItem("Struktura scenarijaSkicaKoncept"),
                                                            new GherkinCompletionItem("Primeri"),
                                                            new GherkinCompletionItem("KontekstOsnovaPozadina")                                                            
                                                        });
            
            _completionDataByLanguage.Add(_langs.GetByIso("sv"),new List<ICompletionData>
                                                        {
                                                        	
                                                            new GherkinCompletionItem("Scenario"),
                                                            new GherkinCompletionItem("Egenskap"),
                                                            new GherkinCompletionItem("När "),
                                                            new GherkinCompletionItem("Så "),
                                                            new GherkinCompletionItem("Givet "),
                                                            new GherkinCompletionItem("Och "),
                                                            new GherkinCompletionItem("Men "),
                                                            new GherkinCompletionItem("Abstrakt Scenario"),
                                                            new GherkinCompletionItem("Exempel"),
                                                            new GherkinCompletionItem("Bakgrund")                                                            
                                                        });
            
            _completionDataByLanguage.Add(_langs.GetByIso("tr"),new List<ICompletionData>
                                                        {
                                                        	
                                                            new GherkinCompletionItem("Senaryo"),
                                                            new GherkinCompletionItem("Özellik"),
                                                            new GherkinCompletionItem("Eğer ki "),
                                                            new GherkinCompletionItem("O zaman "),
                                                            new GherkinCompletionItem("Diyelim ki "),
                                                            new GherkinCompletionItem("Ve "),
                                                            new GherkinCompletionItem("Fakat "),
                                                            new GherkinCompletionItem("Senaryo taslağı"),
                                                            new GherkinCompletionItem("Örnekler"),
                                                            new GherkinCompletionItem("Geçmiş")                                                            
                                                        });
            
            _completionDataByLanguage.Add(_langs.GetByIso("uk"),new List<ICompletionData>
                                                        {
                                                        	
                                                            new GherkinCompletionItem("Сценарій"),
                                                            new GherkinCompletionItem("Функціонал"),
                                                            new GherkinCompletionItem("Якщо "),
                                                            new GherkinCompletionItem("То "),
                                                            new GherkinCompletionItem("Припустимо "),
                                                            new GherkinCompletionItem("І "),
                                                            new GherkinCompletionItem("Але "),
                                                            new GherkinCompletionItem("Структура сценарію"),
                                                            new GherkinCompletionItem("Приклади"),
                                                            new GherkinCompletionItem("Передумова")                                                            
                                                        });
            
            _completionDataByLanguage.Add(_langs.GetByIso("uz"),new List<ICompletionData>
                                                        {
                                                        	
                                                            new GherkinCompletionItem("Сценарий"),
                                                            new GherkinCompletionItem("Функционал"),
                                                            new GherkinCompletionItem("Агар "),
                                                            new GherkinCompletionItem("Унда "),
                                                            new GherkinCompletionItem("Агар "),
                                                            new GherkinCompletionItem("Ва "),
                                                            new GherkinCompletionItem("Лекин "),
                                                            new GherkinCompletionItem("Сценарий структураси"),
                                                            new GherkinCompletionItem("Мисоллар"),
                                                            new GherkinCompletionItem("Тарих")                                                            
                                                        });
            
            _completionDataByLanguage.Add(_langs.GetByIso("vi"),new List<ICompletionData>
                                                        {
                                                        	
                                                            new GherkinCompletionItem("Tình huốngKịch bản"),
                                                            new GherkinCompletionItem("Tính năng"),
                                                            new GherkinCompletionItem("Khi "),
                                                            new GherkinCompletionItem("Thì "),
                                                            new GherkinCompletionItem("Biết "),
                                                            new GherkinCompletionItem("Và "),
                                                            new GherkinCompletionItem("Nhưng "),
                                                            new GherkinCompletionItem("Khung tình huốngKhung kịch bản"),
                                                            new GherkinCompletionItem("Dữ liệu"),
                                                            new GherkinCompletionItem("Bối cảnh")                                                            
                                                        });
            
            _completionDataByLanguage.Add(_langs.GetByIso("zh-CN"),new List<ICompletionData>
                                                        {
                                                        	
                                                            new GherkinCompletionItem("场景"),
                                                            new GherkinCompletionItem("功能"),
                                                            new GherkinCompletionItem("当"),
                                                            new GherkinCompletionItem("那么"),
                                                            new GherkinCompletionItem("假如"),
                                                            new GherkinCompletionItem("而且"),
                                                            new GherkinCompletionItem("但是"),
                                                            new GherkinCompletionItem("场景大纲"),
                                                            new GherkinCompletionItem("例子"),
                                                            new GherkinCompletionItem("背景")                                                            
                                                        });
            
            _completionDataByLanguage.Add(_langs.GetByIso("zh-TW"),new List<ICompletionData>
                                                        {
                                                        	
                                                            new GherkinCompletionItem("場景劇本"),
                                                            new GherkinCompletionItem("功能"),
                                                            new GherkinCompletionItem("當"),
                                                            new GherkinCompletionItem("那麼"),
                                                            new GherkinCompletionItem("假設"),
                                                            new GherkinCompletionItem("而且"),
                                                            new GherkinCompletionItem("但是"),
                                                            new GherkinCompletionItem("場景大綱劇本大綱"),
                                                            new GherkinCompletionItem("例子"),
                                                            new GherkinCompletionItem("背景")                                                            
                                                        });
            
        }

        public void LoadDataInto(IList<ICompletionData> data, Language language)
        {
            _completionDataByLanguage[language]
                .ToList()
                .ForEach(data.Add);
            data.OrderBy(x => x.Text);
        }

        public void LoadLanguages(IList<ICompletionData> data, Languages languages)
        {
            languages.ForEach(l=> data.Add(new GherkinCompletionItem(l.IsoCode, l.English)));
        }
    }
}
