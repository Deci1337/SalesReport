# Dev 1 — Библиотека Model (бизнес-логика)

## Цель
Создать проект-библиотеку `Model` с папками `Core/` и `Data/`, реализующую все классы, интерфейсы и сериализацию.

---

## Задачи

- [ ] **Создать проект** `Model` (Class Library .NET) в solution рядом с `SalesReportApp`.
  Verify: solution содержит два проекта, `SalesReportApp` ссылается на `Model`.

- [ ] **Core/IReportable.cs** — интерфейс с методами:
  ```csharp
  void Sort(bool ascending);
  IEnumerable<ITProduct> Select(Type type);
  ```
  Verify: файл существует, namespace `Model.Core`.

- [ ] **Core/IExportable.cs** — второй интерфейс:
  ```csharp
  string Export(); // возвращает строку-представление отчёта
  ```

- [ ] **Core/ITProduct.cs** — абстрактный класс:
  - Свойства: `Id` (int), `Article` (string), `Brand`, `Model`, `SaleDate` (DateTime?), `BasePrice` (decimal)
  - Абстрактное свойство `Price` (decimal)
  - Перегрузка оператора `==` (по `Article`)
  - Перегрузка метода `ToString()` (возвращает краткое описание)
  Verify: класс abstract, оператор компилируется.

- [ ] **Core/Laptop.cs**, **Core/Smartphone.cs**, **Core/Tablet.cs** — наследуются от `ITProduct`:
  - Laptop: поля `RamGb` (int), `StorageGb` (int) → `Price = BasePrice + RamGb*200 + StorageGb*0.5m`
  - Smartphone: поля `CameraMP` (int), `BatteryMah` (int) → `Price = BasePrice + CameraMP*50 + BatteryMah*0.05m`
  - Tablet: поля `ScreenInch` (double), `HasLTE` (bool) → `Price = BasePrice + ScreenInch*300 + (HasLTE?2000:0)`
  - В каждом переопределить `ToString()` (итого 3+ override)
  Verify: каждый класс override Price и ToString.

- [ ] **Core/Report.cs** — partial класс, реализует `IReportable` и `IExportable`:
  ```csharp
  public string Name { get; set; }
  public DateTime PeriodStart { get; set; }
  public DateTime PeriodEnd { get; set; }
  public List<ITProduct> Products { get; private set; }
  public Report(string name, DateTime start, DateTime end, List<ITProduct> products)
  void Sort(bool ascending)          // сортировка по Article
  IEnumerable<ITProduct> Select(Type type)  // фильтр по типу
  string Export()                    // возвращает строку CSV-подобную
  void AddProduct(ITProduct p)       // добавить одно устройство
  void AddProducts(IEnumerable<ITProduct> items)  // перегрузка (overload)
  void Merge(Report other)           // добавить товары из другого отчёта без дублей
  ```
  Verify: компилируется, Sort/Select работают.

- [ ] **Core/Report.Aggregate.cs** — вторая partial часть:
  ```csharp
  // Конструктор: собирает проданные товары из нескольких отчётов за период
  public Report(string name, IEnumerable<Report> reports, DateTime start, DateTime end)
  ```
  Verify: конструктор без дублей.

- [ ] **Core/Report.PriceChanges.cs** — третья partial часть:
  ```csharp
  // Возвращает массив (SaleDate, AvgPrice) для товара по артикулу
  public PricePoint[] GetPriceChanges(string article)
  ```
  Плюс record/struct `PricePoint { DateTime Date; decimal AvgPrice }`.
  Verify: возвращает отсортированный по дате массив.

- [ ] **Core/DataSeeder.cs** — создаёт тестовые данные:
  - 15–20 устройств (Laptop/Smartphone/Tablet mix)
  - 6 отчётов, каждый с 2–5 устройствами, SaleDate != null у проданных
  - Использует `Func<ITProduct, bool>` для фильтрации при формировании отчётов (делегат)
  - Использует обобщённый метод `Filter<T>(List<ITProduct> list, Predicate<T> pred) where T : ITProduct`
  Verify: 6 объектов Report, устройства не повторяются в разных отчётах.

- [ ] **Data/Serializer.cs** — абстрактный класс:
  ```csharp
  public abstract class Serializer {
      public abstract void Save(IEnumerable<Report> reports, string path);
      public abstract List<Report> Load(string path);
  }
  ```

- [ ] **Data/JsonReportSerializer.cs** — наследует `Serializer`, использует `Newtonsoft.Json`:
  - `Save` → пишет JSON в файл
  - `Load` → читает из файла, если нет — возвращает пустой список
  Verify: файл создаётся/читается без исключений.

- [ ] **Data/XmlReportSerializer.cs** — наследует `Serializer`, использует `System.Xml.Serialization`:
  - Аналогично JSON, но XML-формат
  Verify: `.xml` файл корректен.

- [ ] **Data/SerializerFactory.cs**:
  ```csharp
  public static Serializer Create(string format) // "json" или "xml"
  ```
  Verify: возвращает нужный тип.

---

## Ключевые ООП-требования (проверь сам перед сдачей)

| Требование | Где |
|---|---|
| 2 интерфейса | `IReportable`, `IExportable` |
| 2 абстрактных класса | `ITProduct`, `Serializer` |
| 5+ приведений к базовому/интерфейсу | В `Select()`, `DataSeeder`, `Filter<T>` |
| 3+ override метода/свойства | `Price` ×3, `ToString` ×3 |
| 1+ overload метода | `AddProduct` / `AddProducts` |
| 1 перегрузка оператора | `==` в `ITProduct` |
| 1+ делегат | `Func<>` / `Predicate<>` в `DataSeeder` |
| 1+ generic | `Filter<T>` |

---

## Done When
- [ ] Библиотека `Model` компилируется без ошибок
- [ ] `DataSeeder` создаёт 6+ отчётов, данные корректны
- [ ] JSON и XML сериализация работают (файлы пишутся/читаются)
- [ ] Все классы в правильных папках: `Core/` и `Data/`
