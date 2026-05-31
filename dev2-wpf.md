# Dev 2 — WPF-приложение SalesReportApp (UI)

## Цель
Реализовать интерфейс WPF, подключить библиотеку `Model`, создать `MainWindow`, `ReportWindow`, `PriceChartWindow` и `README.md`.

> Зависит от Dev 1: дождись, пока `Model` компилируется, или создай заглушки классов и работай параллельно.

---

## Задачи

### Подготовка

- [ ] **Добавить ссылку** на проект `Model` в `SalesReportApp`.
  Verify: `using Model.Core;` не даёт ошибок.

- [ ] **Добавить NuGet** `Newtonsoft.Json` и `OxyPlot.Wpf` (или `LiveCharts.Wpf`) в `SalesReportApp`.
  Verify: пакеты установлены, проект собирается.

- [ ] **App.xaml.cs** — при старте вызвать `DataSeeder.Initialize(serializer)`:
  - Если файлы данных не существуют — создать через сериализатор и сохранить
  - Если существуют — загрузить
  Verify: при первом запуске файлы JSON появляются в папке приложения.

---

### MainWindow.xaml

Использовать Grid-разметку. Нужные контролы:

- [ ] **ComboBox** `cbDeviceType` — варианты: "Все", "Ноутбуки", "Смартфоны", "Планшеты"
- [ ] **ComboBox** `cbPeriod` — варианты: "День", "Неделя", "Месяц", "Квартал", "Год"
- [ ] **DatePicker** `dpDate` — выбор даты
- [ ] **ListBox** (с CheckBox) `lbReports` — список отчётов, у которых есть товары в выбранный период; формируется динамически после выбора даты+периода
- [ ] **ComboBox** `cbFormat` — "JSON" / "XML" (выбор формата хранения)
- [ ] **Button** `btnShowReport` — "Показать отчёт", `IsEnabled` привязан к коду:
  - активна, если выбран хотя бы 1 отчёт из `lbReports`
- [ ] **Label** с подписями к каждому элементу

  Verify: все 6 контролов видны, кнопка неактивна по умолчанию.

- [ ] **Логика MainWindow.xaml.cs**:
  - При изменении `dpDate` или `cbPeriod` → вычислить `[start, end]` периода → заполнить `lbReports` отчётами, у которых хотя бы 1 товар с `SaleDate` в диапазоне
  - При изменении `cbFormat` → вызвать `SerializerFactory.Create(newFormat)`, перечитать данные старого формата, записать в новый
  - При клике `btnShowReport` → открыть `ReportWindow`

  Verify: смена периода обновляет список; смена формата создаёт файлы нового типа.

---

### ReportWindow.xaml

- [ ] **DataGrid** `dgProducts` — колонки: Артикул, Бренд, Модель, Тип, Цена, Дата продажи
- [ ] **Button** `btnSortAsc` — "↑ По артикулу"
- [ ] **Button** `btnSortDesc` — "↓ По артикулу"
- [ ] **ComboBox** `cbArticle` — список артикулов из загруженных товаров
- [ ] **Button** `btnPriceChart` — "Динамика цены", активна когда выбран артикул в `cbArticle`
- [ ] **Button** `btnSave` — "Сохранить отчёт"

  Verify: грид заполнен, кнопки сортировки меняют порядок.

- [ ] **Логика ReportWindow.xaml.cs**:
  - Конструктор принимает `List<Report> selectedReports`, `Type deviceType`
  - Собирает сводный `Report` через `new Report(name, selectedReports, start, end)`
  - Применяет `report.Select(deviceType)` и `report.Sort(true)` → отображает в `dgProducts`
  - `btnSortAsc` / `btnSortDesc` → `report.Sort(ascending)` → refresh DataGrid
  - `btnPriceChart` → открыть `PriceChartWindow` с артикулом из `cbArticle`
  - `btnSave` → записать данные таблицы в файл `Отчет_№X_за_{start:dd.MM}-{end:dd.MM}.txt` (или `.csv`)

  Verify: сохранённый файл существует, содержит данные.

---

### PriceChartWindow.xaml

- [ ] Добавить OxyPlot (или LiveCharts) контрол на окно
- [ ] **Логика**:
  - Принять `PricePoint[] points` и `string article`
  - Построить линейный график: ось X = дата продажи, ось Y = средняя цена
  - Заголовок окна: `$"Динамика цены: {article}"`

  Verify: окно открывается, график виден с точками.

---

### README.md (в корне репозитория)

- [ ] Описать:
  - Главное окно: что делает каждый контрол
  - Как формируется список отчётов
  - Как открыть окно с таблицей
  - Как сменить формат файлов
  - Окно отчёта: сортировка, сохранение
  - Кнопка "Динамика цены": когда активна, что показывает

  Verify: файл `README.md` существует в корне, написан по-русски.

---

## Ключевые моменты UI (проверь сам)

| Требование | Где выполнено |
|---|---|
| 3+ разных инструмента визуализации | ComboBox, DatePicker, ListBox(CheckBox), DataGrid, Button, Label |
| Приведение к базовому классу/интерфейсу | В ReportWindow: `IReportable r = report;`, `ITProduct p = item;` (добавь 5+ мест) |
| Обработка исключений | try/catch при чтении/записи файлов, MessageBox с ошибкой |

---

## Структура файлов

```
SalesReportApp/
  App.xaml(.cs)
  MainWindow.xaml(.cs)
  ReportWindow.xaml(.cs)
  PriceChartWindow.xaml(.cs)
Model/
  Core/
    ITProduct.cs
    Laptop.cs  Smartphone.cs  Tablet.cs
    IReportable.cs  IExportable.cs
    Report.cs  Report.Aggregate.cs  Report.PriceChanges.cs
    DataSeeder.cs
  Data/
    Serializer.cs
    JsonReportSerializer.cs
    XmlReportSerializer.cs
    SerializerFactory.cs
README.md
```

---

## Done When
- [ ] Приложение запускается без исключений
- [ ] При первом запуске создаются файлы данных
- [ ] Главное окно фильтрует отчёты по периоду и дате
- [ ] Таблица отчёта отображает правильные товары, сортируется
- [ ] Файл отчёта сохраняется на диск
- [ ] График цены открывается при выбранном артикуле
- [ ] Смена формата JSON↔XML переносит данные
- [ ] README.md заполнен
