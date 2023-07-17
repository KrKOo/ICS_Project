# Téma projektu - Spolujízda

Tématem projektu bude vytvoření aplikace umožňující jejím uživatelů realizovat spolujízdy.

---

## Data

V rámci dat, se kterými se bude pracovat budeme požadovat minimálně následující data.

### Uživatel

- Jméno
- Příjmení
- Fotografie
- (Vlastněná auta)
- (Spolujízdy z pohledu řidiče)
- (Spolujízdy z pohledu spolujezdce)

### Jízda

- Start (místo, poloha)
- Cíl (místo, poloha)
- Čas začátku
- Předpokládaný čas konce, nebo předpokládaná doba cesty
- (Řidič)
- (Spolujezdci)
- (Automobil)

### Auto

- Výrobce
- Typ
- Datum první registrace
- Fotografie
- Počet míst k sezení
- (Majitel, tj. uživatel)

---

## Základní funkcionalita

Aplikace bude obsahovat několik pohledů pro zobrazování přehledu, zobrazení detailu a vložení dat.

Je požadováno **perzistentní** uložení. To znamená, že když se aplikace restartuje, tak nesmí přijít o data. Uložení dat musí být provedeno neprodleně po zadání operace uživatelem.

Při demonstraci bude vyžadováno souběžné spuštění několika aplikací a změny v jedné aplikaci se musí projevit v ostatních instancích. Znovu-načtení dat může být inicializováno uživatelem.

Pro uložení zvolte [SQL Server Express LocalDB](https://docs.microsoft.com/en-us/sql/database-engine/configure-windows/sql-server-express-localdb), která je nainstalována jako součást Visual Studio - Data storage and processing workloadu. Jako ORM framework použijte [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/).

_Minimální_ funkcionalita:

- **Aplikace musí umožnit provést CRUD operace nad všemi daty.**
- **Aplikace se ovládá z pohledu vybraného uživatele při spuštění aplikace.**
- Uživatel může vytvořit jiné uživatele.
- Uživatel může upravit informace o sobě.
- Uživatel může přidat spolujízdu (bude u ní uveden jako řidič).
- Řidič může odebrat spolujezdce a zrušit spolujízdu.
- Uživatel může přidat svá auta a upravovat informace o nich.
- Uživatel vidí seznam spolujízd a může se přihlásit do neobsazené spolujízdy.
- Uživatel může **filtrovat** spolujízdy podle času začátku, míst startu a cíle.

---

### Fáze 1 – objektový návrh

V téhle fázi se zaměříme na _datový návrh_. Vyžaduje se po Vás, aby datový návrh splňoval zadání a nevynechal žádnou podstatnou část. Zamyslete se nad vazbami mezi jednotlivými entitami v datovém modelu. V následující fázi budete entity nahrávat do databáze, takže myslete na jejich propojení již nyní. V této fázi budeme chtít, abyste **odevzdali kód**, kde budete mít _entitní třídy_, které budou obsahovat všechny vlastnosti, které budete dále potřebovat a vazby mezi třídami.

Abyste si vazby dokázali představit, přiložte **ER diagram** vytvořený v libovolném nástroji (i rukou) nebo vygenerovaný z kódu. Pro zajištění vzájemného pochopení všemi členy týmu budeme nově také požadovat vytvoření **wirefame** na všechny pohledy (opět libovolný nástroj či ručně kreslené), které ve vaší výsledné aplikaci chcete implementovat. Tyto wireframy nebudou závazné, ale umožní Vám ihned na začátku vzájemně komunikovat představy o výsledné podobě aplikace. TIP: Při tvorbě wireframe zjistíte, jaká data budete potřebovat a navrhnete korektně nejen vazby v Entitní vrstvě, ale také Modely BL vrstvy, jejichž rozmyšlení jistě oceníte v druhém odevzdání.

ER diagram a wireframy umístěte do kořene repozitáře do adresáře **docs**. Formát souborů zvolte tak, aby se daly otevřít rozumným způsobem bez nutnosti instalace specifických nástrojů. Ideální je obrázek ve formátu png, jpeg, svg atd... případně PDF.

---

### Fáze 2 – databáze, repozitáře a mapování

Vytvořte napojení datových tříd pomocí Entity Frameworku na databázi.

Vytvořte tedy repozitářovou (Repository) vrstvu, která zapouzdří databázové entity a Fasádu, která zpřístupní pouze data přemapovaná do modelů/DTO. **Inspirujte se ve cvičeních anebo vytvořte vlastní infrastrukturu**.

Protože nemáte zatím UI, funkčnost aplikace ověřte automatizovanými testy! Kde to dává logický smysl tvořte **UnitTesty**, pro propojení s databází vytvářejte **Integrační testy**. Pro všechny typy testů využijte libovolný framework, doporučujeme **xUnit**.

Dbejte kvality Vašeho kódu! Opravte si kód odevzdaný v předchozí fázi dle doporučení v review a zásad Clean Code / SOLID, které dále důsledně dodržujte. Můžete si dopomoct např. rozšířením **Code Metrices** a analyzátory kódu.

---

### Fáze 3 – WPF frontend, data binding

V této fázi se od Vás již požaduje vytvoření WPF aplikace.

Napište backend aplikace (ViewModely), který napojíte na Vámi navržené datové modely z 2. fáze, které jsou zapouzdřeny za vrstvou fasád.

A dále frontend (View), který bude zobrazovat data předpřipravená ve ViewModelech. Zamyslete se nad tím, jakým způsobem je vhodné jednotlivá data zobrazovat.

> :warning: **Použití aplikace by mělo být intuitivní.**

Využijte _binding_ v XAML kódu (vyvarujte se code-behind). Účelem není jenom udělat aplikaci, která funguje, ale také aplikaci, která je správně navržena a může být dále jednoduše upravitelná a rozšířitelná. Dbejte tedy zásad probíraných ve cvičeních.
