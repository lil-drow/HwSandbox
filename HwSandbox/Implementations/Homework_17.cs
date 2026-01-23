using HwSandbox.Abstractions;
namespace HwSandbox.Implementations;
/*
 Реализуем паттерн "Прототип"
   Цель:
   Создать иерархию из нескольких классов, в которых реализованы методы клонирования объектов по шаблону проектирования "Прототип".
   
   Описание/Пошаговая инструкция выполнения домашнего задания:
   Придумать и создать 3-4 класса, которые как минимум дважды наследуются и написать краткое описание текстом.
   Создать свой дженерик интерфейс IMyCloneable для реализации шаблона "Прототип".
   Сделать возможность клонирования объекта для каждого из этих классов, используя вызовы родительских конструкторов.
   Составить тесты или написать программу для демонстрации функции клонирования.
   Добавить к каждому классу реализацию стандартного интерфейса ICloneable и реализовать его функционал через уже созданные методы.
   Написать вывод: какие преимущества и недостатки у каждого из интерфейсов: IMyCloneable и ICloneable.
   
   Критерии оценки:
   2 балла: есть краткое описание созданных классов;
   2 балла: реализован шаблон проектирования Prototype с пользовательским интерфейсом;
   2 балла: реализован шаблон проектирования Prototype со стандартным интерфейсом;
   1 балла: созданы тесты/написано тестирование функционала;
   2 балла: написан вывод о преимуществах и недостатках каждого метода;
   1 балл: соблюдение CodeStyle, грамотная архитектура, всё замечания проверяющего исправлены.
   
   Минимальный проходной балл: 8 баллов.
 */
public class Homework_17: Homework
{
    // Планирую рассмотреть реализацию паттерна "Прототип" на примере садоводства.
    // Будем сажать растения: декоративные, съедобные и лечебные
    public Homework_17()
    {
        
    }

    public override void Go()
    {
        Console.WriteLine("Лекция 17. Порождающие паттерны:");
        Console.WriteLine("------------------");
        TestMyCloneable();
        Console.WriteLine("------------------");
        TestDeepCopy();
        Console.WriteLine("------------------");
        CompareInterfaces();
        Console.WriteLine("------------------");
        
        Console.WriteLine("Готово.");
    }

    private void TestMyCloneable()
    {
        Console.WriteLine("1. Тестирование клонирования растений:\n");
        // Создаем оригинальную розу
        var originalRose = new Rose(
            "Роза Королева Елизавета",
            DateTime.Now.Date.AddDays(-30), // Посажена месяц назад
            730, // Живет 2 года
            RoseType.Floribunda,
            "нежно-розовый"
        );
        if (originalRose.State == PlantState.NotPlanted)
        {
            originalRose.Plant(); // Посадить, если еще не посажено
        }
        originalRose.Bloom(); // Роза цветет
        originalRose.Description = "Кустовая роза сорта 'Королева Елизавета' с крупными махровыми цветами.";
        originalRose.Area.Add("Европа");
        originalRose.Area.Add("Азия");
        originalRose.Area.Add("Северная Америка");
        
        Console.WriteLine($"Оригинальная роза:");
        PrintPlantInfo(originalRose);
        Console.WriteLine($"  Цвет: {originalRose.Color}");
        Console.WriteLine($"  Тип розы: {originalRose.RoseType}");
        Console.WriteLine($"  Цветет: {originalRose.IsBlooming}");
        Console.WriteLine($"  Аромат: {originalRose.HasFragrance}");
        
        // Клонируем через IMyCloneable
        var clonedRose = (Rose)originalRose.MyClone();
        Console.WriteLine($"\nКлонированная роза (через IMyCloneable):");
        PrintPlantInfo(clonedRose);
        Console.WriteLine($"  Цвет: {clonedRose.Color}");
        Console.WriteLine($"  Тип розы: {clonedRose.RoseType}");
        Console.WriteLine($"  Цветет: {clonedRose.IsBlooming}");
        Console.WriteLine($"  Аромат: {clonedRose.HasFragrance}");
        
        // Проверяем, что это разные объекты
        Console.WriteLine($"\nПроверки:");
        Console.WriteLine($"Это один и тот же объект? {ReferenceEquals(originalRose, clonedRose)}");
        Console.WriteLine($"Имена одинаковые? {originalRose.Name == clonedRose.Name}");
        Console.WriteLine($"Цвета одинаковые? {originalRose.Color == clonedRose.Color}");
        Console.WriteLine($"Ссылки на список ареалов совпадают? {ReferenceEquals(originalRose.Area, clonedRose.Area)}");
        Console.WriteLine($"Количество ареалов у оригинала: {originalRose.Area.Count}, у клона: {clonedRose.Area.Count}");
    }
    
    private void TestDeepCopy()
    {
        Console.WriteLine("2. Тестирование глубокого копирования:\n");
        
        // Создаем лечебное растение со списком побочных эффектов
        var originalMedicinal = new Medicinal(
            "Эхинацея",
            DateTime.Now.Date,
            200,
            false,
            new List<string> { "Улучшает состояние при болезнях органов дыхания", "Может вызывать сонливость" }
        );
        originalMedicinal.Plant();
        originalMedicinal.Area.Add("Регионы Южного Урала");
        originalMedicinal.Area.Add("Краснодарский край");
        
        Console.WriteLine($"Оригинальное растение:");
        Console.WriteLine($"Название: {originalMedicinal.Name}");
        Console.WriteLine($"Побочные эффекты: {string.Join(", ", originalMedicinal.SideEffectsInfo)}");
        Console.WriteLine($"Области: {string.Join(", ", originalMedicinal.Area)}");
        
        // Клонируем
        var clonedMedicinal = (Medicinal)originalMedicinal.MyClone();
        
        // Модифицируем клон
        clonedMedicinal.SideEffectsInfo.Add("Головокружение");
        clonedMedicinal.Area.Add("Регионы Северной Америки");
        
        Console.WriteLine($"\nПосле модификации клона:");
        Console.WriteLine($"Оригинал - Побочные эффекты: {string.Join(", ", originalMedicinal.SideEffectsInfo)}");
        Console.WriteLine($"Клон - Побочные эффекты: {string.Join(", ", clonedMedicinal.SideEffectsInfo)}");

        Console.WriteLine($"Оригинал - Области: {string.Join(", ", originalMedicinal.Area)}");
        Console.WriteLine($"Клон - Области: {string.Join(", ", clonedMedicinal.Area)}");
        
        Console.WriteLine($"\nПроверки глубокого копирования:");
        Console.WriteLine($"Списки SideEffectsInfo - один объект? {ReferenceEquals(originalMedicinal.SideEffectsInfo, clonedMedicinal.SideEffectsInfo)}");
        Console.WriteLine($"Ссылки на список областей совпадают?{ReferenceEquals(originalMedicinal.Area, clonedMedicinal.Area)}");
    }

    private void CompareInterfaces()
    {
        Console.WriteLine("3. Сравнение IMyCloneable и ICloneable:\n");
        
        var tomato = new Tomato(
            "Черри",
            DateTime.Now.Date,
            110,
            EdibleType.Fruit,
            TomatoType.Cherry
        );
        tomato.Plant();
        PlantBase plantRef = tomato; 
        
        // Сравнение работы интерфейсов
        Console.WriteLine("Сравнение работы интерфейсов:");
        
        // С ICloneable мы получаем object и должны привести
        object objClone = ((ICloneable)plantRef).Clone();
        Tomato tomatoClone = objClone as Tomato;
        Console.WriteLine($"Интерфейс ICLoneable возвращает тип: {tomatoClone.GetType().Name}");
            
        // С IMyCloneable проще
        PlantBase typedClone = plantRef.MyClone();
        Console.WriteLine($"Интерфейс IMyCloneable возвращает тип: {typedClone.GetType().Name}");
        Console.WriteLine("\nОба интерфейса работают корректно.");
    }

    private void PrintPlantInfo(PlantBase plant)
    {
        Console.WriteLine($"  Название: {plant.Name}");
        Console.WriteLine($"  Тип: {plant.GetType().Name}");
        Console.WriteLine($"  Состояние: {plant.State}");
        Console.WriteLine($"  Жизненное состояние: {plant.LifeState}");
        Console.WriteLine($"  Описание: {plant.Description}");
        Console.WriteLine($"  Области распространения: {string.Join(", ", plant.Area)}");
        Console.WriteLine($"  Возраст: {plant.GetAge()} дней");
    }
}
// дженерик интерфейс IMyCloneable для реализации шаблона "Прототип".
public interface IMyCloneable<T>
{
    T MyClone();
}
// интерфейс, описывающий базовые методы высаживаемых объектов
public interface IPlantable
{
    /// <summary>
    /// Посадить растение
    /// </summary>
    void Plant();
    /// <summary>
    /// Убрать растение
    /// </summary>
    void Remove();
    /// <summary>
    /// Статус растения
    /// </summary>
    PlantState State { get; }
}
// энум для описания состояний высаживаемого объекта
public enum PlantState
{
    NotPlanted, // не посажено
    Planted// посажено
}

public enum PlantLifeState
{
    None, // "никакое" - если State == NotPlanted 
    Alive, // живо
    Dead // погибло
}
// базовый абстрактный класс для всех растений
public abstract class PlantBase: IPlantable, IMyCloneable<PlantBase>, ICloneable
{
    /// <summary>
    /// Имя растения
    /// </summary>
    public string Name { get; protected  set; } = string.Empty;
    /// <summary>
    /// Дата посадки
    /// </summary>
    public DateTime PlantDate { get; private set; }

    /// <summary>
    /// Максимальный возраст (в днях)
    /// </summary>
    public int LifeDays { get; private set; }
    private PlantState _state = PlantState.NotPlanted; // изначально растение создаётся не высаженным
    public PlantState State 
    { 
        get => _state;
        private set 
        {
            _state = value;
            // при смене State автоматически обновляем LifeState
            if (_state == PlantState.NotPlanted)
            {
                LifeState = PlantLifeState.None;
            }
        }
    }
    private PlantLifeState _lifeState; 
    /// <summary>
    /// Жизенное состояние растения
    /// </summary>
    public PlantLifeState LifeState 
    { 
        get => _lifeState;
        protected set 
        {
            // не позволяем установить LifeState кроме None, если растение не посажено
            if (State == PlantState.NotPlanted && value != PlantLifeState.None)
            {
                throw new InvalidOperationException(
                    "Нельзя установить жизненное состояние для растения, которое не посажено!");
            }
            _lifeState = value;
        }
    }
    /// <summary>
    /// Описание растения
    /// </summary>
    public string Description { get; set; } = string.Empty;
    /// <summary>
    /// Места нахождения растения
    /// </summary>
    public List<string> Area { get; protected set; } = new List<string>();
    
    public PlantBase(string name, DateTime plantDate, int lifeDays)
    {
        Name = name;
        PlantDate = plantDate;
        LifeDays = lifeDays;

        if (plantDate < DateTime.Now.Date)
        {
            State = PlantState.Planted;
            LifeState = PlantLifeState.Alive;
        }
    }
    // конструктор для глубокого копирования
    public PlantBase(PlantBase plant)
    {
        Name = plant.Name;
        PlantDate = plant.PlantDate;
        LifeDays = plant.LifeDays;
        State = plant.State;
        LifeState = plant.LifeState;
        Description = plant.Description;
        Area = new List<string>(plant.Area);
    }
    // ожидается, что метод будет переопределён при наследовании
    public virtual PlantBase MyClone()
    {
        throw new NotImplementedException("Переопределите метод в наследном классе!");
    }

    object ICloneable.Clone()
    {
        return MyClone();
    }
    public virtual void Plant()
    {
        State = PlantState.Planted;
        PlantDate = DateTime.Now.Date;
        LifeState = PlantLifeState.Alive;
    }

    public virtual void Remove()
    {
        State = PlantState.NotPlanted;
    }
    /// <summary>
    /// Узнать возраст растения
    /// </summary>
    /// <returns></returns>
    public virtual int GetAge()
    {
        switch (LifeState)
        {
            case PlantLifeState.Alive:
                {
                    int days = (DateTime.Now.Date - PlantDate).Days;
                    if (days > LifeDays)
                    {
                        LifeState = PlantLifeState.Dead;
                        // состарившееся до смерти растение больше не стареет
                        return LifeDays;
                    }
                    return days;
                }
            case PlantLifeState.Dead:
                return LifeDays;
            default:
                return 0;
        }
    }
}
/// <summary>
/// Декоративное растение
/// </summary>
public class Ornamental : PlantBase
{
    public bool IsBlooming { get; private set; } // цветёт 
    public bool HasFragrance { get; private set; } // и пахнет
    public Ornamental(string name, DateTime plantDate, int lifeDays, bool hasFragrance) 
        : base(name, plantDate, lifeDays)
    {
        HasFragrance = hasFragrance;
    }
    // конструктор для глубокого копирования
    public Ornamental(Ornamental ornamentalPlant) : base(ornamentalPlant)
    {
        IsBlooming = ornamentalPlant.IsBlooming;
        HasFragrance = ornamentalPlant.HasFragrance;
    }

    public override PlantBase MyClone()
    {
        return new Ornamental(this);
    }
    
    public void Bloom()
    {
        switch (LifeState)
        {
            // цветут только живые растения
            case PlantLifeState.Alive:
                IsBlooming = true;
                break;
            default:
                if (IsBlooming)
                    IsBlooming = false;
                break;
        }
    }
}
/// <summary>
/// Классификация растения по используемым частям
/// </summary>
public enum EdibleType
{
    /// <summary>
    /// Листовое растение
    /// </summary>
    Leafy, 
    /// <summary>
    /// Плодовое растение
    /// </summary>
    Fruit,
    /// <summary>
    /// Корнеплод
    /// </summary>
    Root,
    /// <summary>
    /// Луковичное
    /// </summary>
    Bulbous // луковичные
}
/// <summary>
/// Съедобное растение
/// </summary>
public class Edible : PlantBase
{
    public EdibleType EdibleType { get; private set; }
    public bool IsHarvestable { get; private set; }
    public Edible(string name, DateTime plantDate, int lifeDays, EdibleType edibleType) 
        : base(name, plantDate, lifeDays)
    {
        EdibleType = edibleType;
    }
    // конструктор для глубокого копирования
    public Edible(Edible ediblePlant) : base(ediblePlant)
    {
        EdibleType = ediblePlant.EdibleType;
        IsHarvestable = ediblePlant.IsHarvestable;
    }
    public override PlantBase MyClone()
    {
        return new Edible(this); 
    }
    public void Harvest()
    {
        switch (LifeState)
        {
            case PlantLifeState.Alive:
                IsHarvestable = true;
                break;
            case PlantLifeState.Dead:
                break; // если растение принесло урожай до своей гибели, его ещё можно собрать
            default:
                if (IsHarvestable)
                    IsHarvestable = false;
                break;
        }
    }
}
/// <summary>
/// Результат приготовления лекарства из лечебного растения
/// </summary>
public enum MedicinePrepareResult
{
    None,
    Success,
    Failure
}
/// <summary>
/// Лечебное растение
/// </summary>
public class Medicinal : PlantBase
{
    public bool IsToxic { get; private set; } // для простоты планирую отмечать как true только сильно токсичные растения
    public List<string> SideEffectsInfo { get; protected set; } // инфо о побочных эффектах применения
    public Medicinal(string name, DateTime plantDate, int lifeDays, bool isToxic, List<string> sideEffectsInfo) 
        : base(name, plantDate, lifeDays)
    {
        IsToxic = isToxic;
        SideEffectsInfo = sideEffectsInfo;
    }
    // конструктор для глубокого копирования
    public Medicinal(Medicinal medicinalPlant) : base(medicinalPlant)
    {
        IsToxic = medicinalPlant.IsToxic;
        SideEffectsInfo = new List<string>(medicinalPlant.SideEffectsInfo);
    }
    public override PlantBase MyClone()
    {
        return new Medicinal(this);
    }
    public MedicinePrepareResult PrepareMedicine()
    {
        switch (LifeState)
        {
            case PlantLifeState.Alive:
            {
                Remove(); // для простоты буду считать, что растение используется полностью
                return MedicinePrepareResult.Success;
            }
            case PlantLifeState.Dead:
            {
                Remove(); // мёртвое растение тоже можно использовать, но лекарство из него не получится.
                return MedicinePrepareResult.Failure;
            }
            default:
                return MedicinePrepareResult.None;
                
        }
    }
}
/// <summary>
/// Тип розы
/// </summary>
public enum RoseType
{
    /// <summary>
    /// Роза чайная
    /// </summary>
    Tea,
    /// <summary>
    /// Роза полиантовая
    /// </summary>
    Polyantha,
    /// <summary>
    /// Роза "Флорибунда"
    /// </summary>
    Floribunda,
    /// <summary>
    /// Роза китайская
    /// </summary>
    Chinese,
}
public class Rose : Ornamental
{
    public string Color { get; private set; }
    public RoseType RoseType { get; private set; }

    public Rose(string name, DateTime plantDate, int lifeDays, RoseType roseType, string color)
        : base(name, plantDate, lifeDays, hasFragrance: true)
    {
        RoseType = roseType;
        Color = color;
    }

    public Rose(Rose rosePlant) : base(rosePlant)
    {
        RoseType = rosePlant.RoseType;
        Color = rosePlant.Color;
    }
    public override PlantBase MyClone()
    {
        return new Rose(this);
    }
}
/// <summary>
/// Тип томата
/// </summary>
public enum TomatoType
{
    /// <summary>
    /// Томат "Абаканский"
    /// </summary>
    Abakan,
    /// <summary>
    /// Томат "Бычье сердце"
    /// </summary>
    BullsHeart,
    /// <summary>
    /// Томат "Черри"
    /// </summary>
    Cherry,
    /// <summary>
    /// Томат "Чернолапка"
    /// </summary>
    BlackPaw,
    /// <summary>
    /// Томат "Золотые купола"
    /// </summary>
    GoldenDomes
}
public class Tomato : Edible
{
    public TomatoType TomatoType { get; private set; }

    public Tomato(string name, DateTime plantDate, int lifeDays, EdibleType edibleType, TomatoType tomatoType)
        : base(name, plantDate, lifeDays, edibleType)
    {
        TomatoType = tomatoType;
    }
    public Tomato(Tomato tomatoPlant) : base(tomatoPlant)
    {
        TomatoType = tomatoPlant.TomatoType;
    }
    public override PlantBase MyClone()
    {
        return new Tomato(this); 
    }
}
/// <summary>
/// Тип мяты
/// </summary>
public enum MenthaType
{
    /// <summary>
    /// Мята перечная
    /// </summary>
    Piperita,
    /// <summary>
    /// Мята душистая
    /// </summary>
    Suaveolens,
    /// <summary>
    /// Мята болотная
    /// </summary>
    Pulegium
}
public class Mentha : Medicinal
{
    public MenthaType MenthaType { get; private set; }

    public Mentha(string name, DateTime plantDate, int lifeDays, List<string> sideEffectsInfo, MenthaType menthaType)
        : base(name, plantDate, lifeDays, isToxic: false, sideEffectsInfo)
    {
        MenthaType = menthaType;
    }

    public Mentha(Mentha menthaPlant) : base(menthaPlant)
    {
        MenthaType = menthaPlant.MenthaType;
    }
    public override PlantBase MyClone()
    {
        return new Mentha(this);
    }
}

