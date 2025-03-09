

using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

public static class lab2
{
    public static void Main()
    {
        Console.Write("N = ");
        UInt16 N = Convert.ToUInt16(Console.ReadLine());
        Console.Write("\nM = ");
        UInt16 M = Convert.ToUInt16(Console.ReadLine());
        Console.Write("\nt1 = ");
        UInt16 T1 = Convert.ToUInt16(Console.ReadLine());
        Console.Write("\nt2 = ");
        UInt16 T2 = Convert.ToUInt16(Console.ReadLine());
        Console.Write("\nБарьер по максимуму - 1, Барьер по минимумам - 0");
        int barier_type = Convert.ToInt32(Console.ReadLine());


        var test = new Matrix(N, M, T1, T2);
        test.createMatrix();
        test.BarrierNum(barier_type);
        test.PrintMatrix(barier_type);

        Console.Write("\nВведите 0, если хотите сортировку по возрастанию, 1 - по убыванию, или что иное, чтоб оставить матрицу исходной");
        int desc = Convert.ToUInt16(Console.ReadLine());
        test.SortMatrix(desc);
        test.PrintMatrix(barier_type);

        var task = new MainTask(test);
        task.Main();
        
    }
}

public class Matrix
{
    public UInt16 N { get; set; }
    public UInt16 M { get; set; }
    public UInt16 T1 { get; set; }
    public UInt16 T2 { get; set; }
    public List<List<int>> matrix { get; set; }

    public int barrier;

    public Matrix(UInt16 n, UInt16 m, UInt16 t1 = 10, UInt16 t2 = 25)
    {
        N = n;
        M = m;
        T1 = t1;
        T2 = t2;
    }

    public void createMatrix()
    {
        matrix = new List<List<int>>();
        var rand = new Random();
        for (int i = 0; i < M; i++)
        {
            List<int> tmpstr = new List<int>();
            for (int j = 0; j < N; j++)
            {
                tmpstr.Add(rand.Next(T1, T2));
            }
            matrix.Add(tmpstr);
        }
    }

    public void BarrierNum(int max_min)
    {
        int barr_val = 0;
        if (max_min == 0)
        {
            Console.WriteLine("Барьер расчитывается по минимумам.");
            foreach (var el in matrix)
            {
                barr_val += el.Min();
            }
        }
        else
        {
            Console.WriteLine("Барьер расчитывается по максимумам.");
            foreach (var el in matrix)
            {
                barr_val += el.Max();
            }
        }
            barrier = (int)Math.Ceiling((double)barr_val / N);
    }

    public void PrintMatrix(int max_min)
    {
        if (max_min == 0)
        {
            foreach (var el in matrix)
            {
                string answer = string.Join(" ", el);
                answer += " T= " + el.Sum().ToString();
                answer += " Min= " + el.Min().ToString();
                Console.WriteLine(answer);
            }
        }
        else
        {
            foreach (var el in matrix)
            {
                string answer = string.Join(" ", el);
                answer += " T= " + el.Sum().ToString();
                answer += " Max= " + el.Min().ToString();
                Console.WriteLine(answer);
            }
        }
            Console.WriteLine("Значение барьера = " + barrier);
        Console.WriteLine();
    }

    public void SortMatrix(int desc = 0)
    {
        switch (desc)
        {
            case 0:
                matrix = matrix.OrderBy(str => str.Sum()).ToList();
                break;
            case 1:
                matrix = matrix.OrderByDescending(str => str.Sum()).ToList();
                break;
            default:
                break;
        }
    }
}

public class MainTask {
    public UInt16 N { get; init; }
    public UInt16 M { get; init; }
    public List<List<int>> matrix { get; init; }
    private int[] tasks { get; set; }

    public int barier_num; 

    public MainTask(Matrix mat) {
        N = mat.N;
        M = mat.M;
        matrix = mat.matrix;
        tasks = new int[N];
        for (int i = 0; i < N; i++)
        {
            tasks[i] = 0;
        }
        barier_num = mat.barrier;
    }

    public void Main()
    {
        foreach(var el in matrix)
        {
            int[] tmp_task = tasks.ToArray();
            var index_min_before = el.IndexOf(el.Min());
            tmp_task[index_min_before] += el.Min();
            if (tmp_task.Max() <= barier_num) {
                Console.WriteLine("Минимальный элемент: " + el.Min());
                Console.WriteLine("P = {" + String.Join(",", tmp_task) + "}");
                tasks = tmp_task.ToArray();
            }
            else
            {
                Console.WriteLine("Действия после барьера");
                List<int> tmp = new List<int>();
                for (int j = 0; j < N; j++)
                {
                    tmp.Add(el[j] + tasks[j]);
                }
                Console.WriteLine("Строка памяти: " + string.Join(",", tmp));
                var indexmin = tmp.IndexOf(tmp.Min());
                Console.WriteLine("Удовлетворительный элемент: " + el[indexmin] + " + " + tasks[indexmin]);
                tasks[indexmin] += el[indexmin];
                Console.WriteLine("P = {" + String.Join(",", tasks) + "}");
            }
        }

        Findmax();
    }

    private void Findmax()
    {
        Console.WriteLine("max(" + String.Join(",", tasks) + ") = " + tasks.Max().ToString());
    }
}
