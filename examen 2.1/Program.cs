using System;
using System.Data.SqlClient;

class Program
{
    static int c1 = 0, c4 = 0, cM = 0, cont = 0;
    static int sumM = 0, sumF = 0, sumNeto = 0;

    static string connectionString = "Data Source=NombreDelServidor;Initial Catalog=NombreDeLaBaseDeDatos;Integrated Security=True";

    static void Ingreso()
    {
        double porc, precio, bruto, dcto, neto;
        int tipo, cant;
        char gen;
        Console.WriteLine();
        do
        {
            Console.Write("Genero (M:masculino F:femenino) : ");
            gen = Console.ReadKey().KeyChar;
            gen = char.ToUpper(gen);
            Console.WriteLine();
        } while (gen != 'M' && gen != 'F');
        do
        {
            Console.Write("Tipo de Libro (1:Ficcion 2:Novelas 3:Cuentos 4:Fisica Cuantica) : ");
            tipo = int.Parse(Console.ReadLine());
        } while (tipo < 0 || tipo > 4);
        do
        {
            Console.Write("Cantidad de libros : ");
            cant = int.Parse(Console.ReadLine());
        } while (cant <= 0);

        switch (tipo)
        {
            case 1: precio = 90; break;
            case 2: precio = 100; break;
            case 3: precio = 80; break;
            case 4: precio = 150; break;
            default: precio = 0; break;
        }

        if (cant <= 2)
        {
            switch (tipo)
            {
                case 1: porc = 0.05; break;
                case 2: porc = 0.08; break;
                case 3: porc = 0.09; break;
                case 4: porc = 0.02; break;
                default: porc = 0; break;
            }
        }
        else if (cant <= 6)
        {
            switch (tipo)
            {
                case 1: porc = 0.06; break;
                case 2: porc = 0.16; break;
                case 3: porc = 0.18; break;
                case 4: porc = 0.02; break;
                default: porc = 0; break;
            }
        }
        else
        {
            switch (tipo)
            {
                case 1: porc = 0.08; break;
                case 2: porc = 0.32; break;
                case 3: porc = 0.36; break;
                case 4: porc = 0.04; break;
                default: porc = 0; break;
            }
        }

        bruto = cant * precio;
        dcto = bruto * porc;
        neto = bruto - dcto;

        Console.WriteLine();
        Console.WriteLine("Importe a pagar : " + bruto);
        Console.WriteLine("Descuento : " + dcto);
        Console.WriteLine("Importe Neto : " + neto);

        if (tipo == 4)
        {
            c4++;
        }
        if (tipo == 1 && porc == 0.06)
        {
            c1++;
        }
        if (gen == 'M' && dcto >= 200 && dcto <= 2500)
        {
            cM++;
        }
        sumNeto += (int)neto;

        if (gen == 'F' && tipo == 2)
        {
            sumF += (int)neto;
        }

        if (gen == 'M' && tipo == 3)
        {
            sumM += (int)neto;
            cont++;
        }

        // Guardar los datos en la base de datos
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string query = "INSERT INTO lectores (nombre, genero, titulo, cantidad) VALUES (@nombre, @genero, @titulo, @cantidad)";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@nombre", "");
            command.Parameters.AddWithValue("@genero", gen.ToString());
            command.Parameters.AddWithValue("@titulo", tipo.ToString());
            command.Parameters.AddWithValue("@cantidad", cant);
            command.ExecuteNonQuery();
        }
    }

    static void Reporte()
    {
        double prom;
        if (cont > 0)
            prom = sumM / cont;
        else
            prom = 0;
        Console.WriteLine();
        Console.WriteLine("------ REPORTE ------");
        Console.WriteLine("Cantidad ventas de Fisica Cuantica : " + c4);
        Console.WriteLine("Cantidad ventas de Ficcion y dcto 6% : " + c1);
        Console.WriteLine("Cantidad ventas Varones y dcto [200,2500] : " + cM);
        Console.WriteLine("Total Importe Neto : " + sumNeto);
        Console.WriteLine("Total Neto Mujeres y Novelas : " + sumF);
        Console.WriteLine("Promedio Neto de Varones y Cuentos : " + prom);
    }

    static void Main()
    {
        int opcion;
        do
        {
            Console.WriteLine();
            Console.WriteLine("----------------------");
            Console.WriteLine(" MENU ");
            Console.WriteLine("----------------------");
            Console.WriteLine("[1] Registrar Venta ");
            Console.WriteLine("[2] Reportar Venta ");
            Console.WriteLine("[3] Salir ");
            Console.WriteLine("----------------------");
            Console.WriteLine();
            Console.Write(" Opcion : ");
            opcion = int.Parse(Console.ReadLine());
            Console.WriteLine();
            if (opcion == 3)
            {
                Console.WriteLine("Fin del Programa");
            }
            if (opcion > 3)
            {
                Console.WriteLine("ERROR !!!! , DEBE INGRESAR UNA OPCION DEL 1 al 3");
            }
            switch (opcion)
            {
                case 1:
                    Ingreso();
                    break;
                case 2:
                    Reporte();
                    break;
                default:
                    break;
            }
        } while (opcion != 3);
        Console.WriteLine("FIN DE PROCESO");
    }
}