using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DataAccesLib.DataAccess;
using DataAccesLib.Models;
using VAII.Models;

namespace VAII.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DataContext _context;

        public HomeController(DataContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult About()
        {
            var teamMembers = _context.TeamMembers.ToList();
            if (teamMembers.Count <= 1)
            {
                
            }
            return View(teamMembers);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult SmartNews()
        {
            if (!_context.SmartNews.Any())
            {
                var itemList = new List<SmartBlogItem>();
                itemList.Add( new SmartBlogItem()
                {
                    ImagePath = "https://touchit.sk/wp-content/uploads/2020/04/galaxy_s20_testy_1.jpg",
                    Author = "SmartMedic",
                    Heading = "Samsung v tichosti ukončil predaj série Galaxy S20",
                    Created = new DateTime(2020, 1, 12),
                    text = "Je prirodzené, že po predstavení noviniek niektorí výrobcovia smartfónov ukončia svoje produktové línie skôr.\r\n\r\nTo samozrejme neplatí pre každú spoločnosť. Ak je zariadenie úspešné, má tendenciu sa dlho udržať na trhu, kým sa exponenciálne nezníži predaj, alebo kým používatelia nezačnú viac kupovať novší model. Apple je známy tým, že na trhu drží svoje smartfóny dlho. V skutočnosti má značka tendenciu znižovať cenu starých modelov. Používatelia si ho preto môžu dovoliť kúpiť, ak je cena pre nich dostatočne atraktívna.\r\n\r\nSamsung nikdy nebol “katom” svojich smartfónov a predaj nepozastavil tak rýchlo, ako tento rok. Z oficiálnych webových stránok spoločnosti Samsung už nie je možné kúpiť žiadny model pôvodnej série Galaxy S20."

                }
                );

                itemList.Add(new SmartBlogItem()
                    {
                        ImagePath = "https://touchit.sk/wp-content/uploads/2021/01/TCL_CES2021_Printed-OLED-Scrolling_Display_Stretched_nowat.png",
                    Author = "SmartMedic",
                        Heading = "Na čo sú dobré rolovateľné displeje?",
                        Created = new DateTime(2020, 12, 3),
                        text = "CES 2021 bol aj v znamení rolovateľných displejov. Už pred rokom sme videli televízory LG, ktoré mali takéto displeje, LG rovnako ukázalo teraz aj smartfón s takouto vlastnosťou. Ale niečo podobné sme videli aj na virtuálnom stánku TCL.\r\n\r\nAko prvý to bol 17-palcový rolovateľný displej OLED a druhým je 6,7-palcový rolovateľný displej AMOLED. Na CESe ich prezentovala spoločnosť TCL CSOT, čo je dcéra TCL Technology s orientáciou na inovácie na poli polovodičových displejov.\r\n\r\nOhybný 17-palcový tlačený rolovateľný displej typu OLED má hrúbku len 0,18 mm a je vzorom pre pre veľké ohybné displeje. Je plne rolovateľný, podobne ako maliarske plátno a takto „zmotaný“ displej sa vojde doslova kamkoľvek. Displej tak môžete nosiť v aktovke. "

                    }
                );
                itemList.Add(new SmartBlogItem()
                    {
                        ImagePath = "https://touchit.sk/wp-content/uploads/2021/01/HiSense6_CES2021_nowat.jpg",
                        Author = "SmartMedic",
                        Heading = " Hisense predstaví nové televízory aj na Slovensku",
                        Created = new DateTime(2020, 12, 3),
                        text = " Tohtoročný veľtrh spotrebnej elektroniky CES naznačil smerovanie štvrtého najväčšieho výrobcu TV na svete. Tým je kvalitný obraz vďaka laserovej technológii TriChroma a ešte veľmi krátka doba odozvy daná najrýchlejším operačným systémom VIDAA U5 s profesionálnym herným režimom Game Mode Pro.\r\n\r\n\r\n\r\nNechýbajú ani novinky ako bionická laserová TV so zvukom vychádzajúcim priamo z obrazovky, 8K TV a široká ponuka špičkovo vybavených 4K ULED TV pre globálny trh na čele s Mini LED TV. Filmoví, športoví i herní fanúšikovia sa tak majú na čo tešiť. Veríme, že s formou HiSense nadviažeme dobré vzťahy a budete sa môcť tešiť na naše recenzie televízorov tejto značky."

                    }
                );
                itemList.Add(new SmartBlogItem()
                    {
                        ImagePath = "https://images.idgesg.net/images/article/2020/08/android-awkward-timing-100855433-large.jpg",
                    Author = "SmartMedic",
                        Heading = "Prvé detaily prezrádzajú, ako bude fungovať hibernácie aplikácií v Androide 12",
                        Created = new DateTime(2020, 12, 3),
                        text = "Stále nie je jasné, či bude funkcia pripravená načas. Prvé náznaky o novinke sa objavili pred pár dňami. Android 11 sa na niektorých smartfónoch ani poriadne neohrial a už sa dozvedáme prvé detaily o jeho nástupcovi. Pokiaľ pôjde všetko podľa plánu, tak prvý vývojárský náhľad Androidu 12 by sme mohli spoznať už budúci mesiac. Doposiaľ sme sa dozvedeli, že bude umožnené jednoduchšie používanie alternatívnych obchodov s aplikáciami, ale najnovšia správa znie obzvlášť zaujímavo. Google pracuje na novej systémovej službe, ktorá uvedie nečinné aplikácie do hlbokého spánku, čím by sa umožnila optimalizácia úložiska. Systémová služba môže spravovať nečinné aplikácie a pokiaľ sa aktívne nepoužívajú, je ich možné optimalizovať. Podľa poslednej príslušnej zmeny kódu, bude nadchádzajúca funkcia hibernácie aplikácií pre Android fungovať odlišne v závislosti od toho, či je povolená pre jedného používateľa alebo pre všetkých používateľov. Ak je iba jeden používateľ na zariadení s viacerými používateľmi, vymaže sa vyrovnávacia pamäť aplikácií pre konkrétneho používateľa. Úlohy uvedené v kóde naznačujú, že ďalším krokom bude podpora hibernácie na úrovni balíka, ktorá ovplyvní všetkých používateľov, aj keď nie je jasné, ako sa to bude líšiť. Je možné, že všetky údaje aplikácie budú vymazané, alebo že samotná aplikácia bude zo zariadenia skutočne odstránená a bude znovu nainštalovaná, keď používateľ vypne režim dlhodobého spánku."

                }
                );
                itemList.Add(new SmartBlogItem()
                    {
                        ImagePath = "https://cdn.wccftech.com/wp-content/uploads/2020/12/Samsung-Exynos-Logo-Illustration-AH-DB.jpg",
                    Author = "SmartMedic",
                        Heading = "Samsung pripravuje konkurenciu pre procesor M1 od Applu",
                        Created = new DateTime(2020, 12, 3),
                        text = "Apple minulý rok oficiálne vydal prvý samostatne vyvinutý čip Apple M1 pre Mac. Vďaka vysokému výkonu a vynikajúcej energetickej efektívnosti na seba ihneď upozornil. Podľa portálu GizChina by spoločnosť Samsung mala vo štvrtom štvrťroku tohto roka predstaviť Exynos čipset pre PC. Bude založený na najnovšom dizajne Samsung Exynos 2100 a bude vyrobený 5nm technológiou. Okrem toho bude využívať GPU od AMD. Samotný Exynos 2100 je vyrobený najnovším 5nm procesom EUV spoločnosti Samsung a je vybavený super veľkým jadrom Cortex-X1 s taktom 2,9 GHz, doplneným o 3 veľké 2,8 GHz A78 jadrá a 4 malé 2,2 GHz A55 jadrá. Stojí za zmienku, že už v roku 2019 spoločnosti Samsung a AMD odštartovali viacročnú strategickú spoluprácu. Uviedli, že obe strany sa budú usilovať o ďalší rozvoj v oblasti ultranízkej spotreby energie a spracovania obrazu. S technickou podporou AMD sa nakoniec môžeme skutočne dočkať výsledkov, ktoré budú konkurencieschopné aj čipu Apple M1."

                }
                );
                itemList.Add(new SmartBlogItem()
                    {
                        ImagePath = "https://cdn.onlinewebfonts.com/svg/img_70422.png",
                    Author = "SmartMedic",
                        Heading = "Obchod Play: Nová ikona prezradí, ktorým smerom sa uberá popularita aplikácií",
                        Created = new DateTime(2020, 12, 3),
                        text = " Uvidíte, či sa im darí alebo ich popularita klesá. Poradie aplikácií je viditeľné na výsledkovej tabuli a Google týmto spôsobom ukazuje vývojárom, vydavateľom aj potenciálnym používateľom, ako sa ich aplikáciám aktuálne darí. Pomocou novej ikony je možné aj predpokladať, kam bude smerovať popularita medzi najlepšími aplikáciami v Obchode Play. Portál AndroidPolice informuje, že vďaka novým ikonám na karte Rebríčky najlepších vo vyhľadávaní uvidíte, ktoré aplikácie majú predpoklad stúpať, a ktoré v popularite sťahovania klesajú. Pri aplikáciách alebo hrách chýba chýba časové obdobie, za ktoré sa popularita hodnotí. Zatiaľ je to označené veľmi jednoducho, ale zrozumiteľne. Či sa bude tento ukazovateľ vývoja aplikácií ešte meniť, alebo či prinesie pre Google alebo vývojárov (používateľov) želaný efekt, ukáže až čas."

                }
                );
                _context.SmartNews.AddRange(itemList);
                _context.SaveChanges();
            }

            return View(_context.SmartNews);
        }

        public IActionResult NewsDetail(int? id)
        {
           
            var viewItem = _context.SmartNews.FirstOrDefault(n => n.id == id);

            return View(viewItem);
        }
    }
}
