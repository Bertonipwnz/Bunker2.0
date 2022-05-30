using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DSharpPlus;
using Бункер2._0.Классы;

namespace Бункер2._0
{

    /// <summary>
    /// Игра написана по мотивам настольной игры "Бункер"
    /// Характеристики генерируется рандомно и записываются в файл, с номером игрока
    /// </summary>
    public partial class Form1 : Form
    {
        // //глобальный переменные
        int kolvo = 0; //Инициализация переменной для количества игроков
        string Put; //Инициализация переменной для хранения ссылки на путь к файлам игроков
        readonly Random rnd = new Random(); //иницилизируем переменную типа рандом и выделяем для неё память в куче
        readonly Game character = new Game(); //ссылка на объект в куче и создание экземпляра класса
        //массив характеристик для записи
        string[] oborudovano;
        string[] estInBunker;
        string[] proffesionRnd;
        string[] healthRnd;
        string[] phobiaRnd;
        string[] hobbyRnd;
        string[] HTR;
        string[] luggageRnd;
        string[] addInfoRnd;
        string[] kartochka1Rnd;
        public Form1()
        {
            InitializeComponent();
            
        }
        private void button1_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog(); //вызов окна с обзором пути
            Put = folderBrowserDialog1.SelectedPath; //считываем путь к папке
            kolvo = Convert.ToInt32(textBox1.Text); //конвертируем поле с кол-вом игроков считывающихся с textBox1
            string filepath; //переменная для пути к папке
            proffesionRnd = character.ShuffleAndRandom(Game.profession.Length, Game.profession);
            healthRnd = character.ShuffleAndRandom(Game.health.Length, Game.health);
            phobiaRnd = character.ShuffleAndRandom(Game.phobia.Length, Game.phobia);
            hobbyRnd = character.ShuffleAndRandom(Game.hobby.Length, Game.hobby);
            HTR = character.ShuffleAndRandom(Game.humanTraitRand.Length, Game.humanTraitRand);
            luggageRnd = character.ShuffleAndRandom(Game.luggage.Length, Game.luggage);
            addInfoRnd = character.ShuffleAndRandom(Game.additionalInformation.Length, Game.additionalInformation);
            kartochka1Rnd = character.ShuffleAndRandom(Game.kartochka1.Length, Game.kartochka1);
            oborudovano = character.ShuffleAndRandom(Game.oborudovanieBunker.Length, Game.oborudovanieBunker);
            estInBunker = character.ShuffleAndRandom(Game.estInBunker.Length, Game.estInBunker);
            string[] gender = Game.gender;
            int shkolnikID = rnd.Next(1, kolvo); //переменная для формирования школьника
            int dedID = rnd.Next(1, kolvo); //переменная для формирования деда/бабушки
            while (shkolnikID == dedID) //цикл на проверку совпадений номеров игрококов, если повторяется вызываем рандом ещё раз
            {
                shkolnikID = rnd.Next(1, kolvo);
                dedID = rnd.Next(1, kolvo);
            }
            //цикл формирование документов и записи в них данных игроков
            for (int numberplayer = 1, counter = 0, kart = kolvo; numberplayer < kolvo + 1; kart++, numberplayer++, counter++)//номер игрока, счётчик данных, чтобы не было повторений kart;
            {
                filepath = folderBrowserDialog1.SelectedPath + @"\" + numberplayer + ".txt"; //записываем в переменную номер игрока - номер документа
                StreamWriter f = new StreamWriter(filepath, false); //создаём экземпляр и передаём filepath, false - создаёт новый документ если нету
                int age = character.age(23, 50); //вызываем функцию для определения возраста и записываем в переменную age
                if (numberplayer == shkolnikID || numberplayer == dedID) //формирования отдельных карточек среди общих для школьника/деда
                {
                    if (numberplayer == shkolnikID)
                    {
                        //школьник
                        int shkolaAge = character.age(14, 18);
                        f.WriteLine("Желание работать или работает(на ваш выбор): " + proffesionRnd[counter]);
                        f.WriteLine("Пол: " + gender[rnd.Next(0, 2)]);
                        f.WriteLine("Возраст: " + shkolaAge + " лет");
                        f.WriteLine("Пассивка школьника(вы не можете выжить без бабушки/дедушки) под номером: " + dedID);
                        f.WriteLine("Багаж школьника+: " + luggageRnd[kart]);
                        f.WriteLine("Хобби: " + hobbyRnd[counter] + ". Стаж хобби: " + character.expHobby(shkolaAge) + " лет");
                    }
                    else
                    {
                        //дед/бабушка
                        int starikAge = character.age(60, 95);
                        f.WriteLine("Профессия: " + proffesionRnd[counter] + " Стаж работы: " + character.expDedWork(starikAge));
                        f.WriteLine("Профессия 2: " + proffesionRnd[kart] + " Стаж работы: " + character.expDedWork(starikAge));
                        f.WriteLine("Пол: " + gender[rnd.Next(0, 2)]);
                        f.WriteLine("Возраст: " + starikAge + " лет");
                        f.WriteLine("Ваш внук/внучка под номером: " + shkolnikID);
                        f.WriteLine("Хобби: " + hobbyRnd[counter] + ". Стаж хобби: " + character.expHobby(starikAge) + " лет");
                    }
                }
                else
                {
                    //для остальных игроков
                    f.WriteLine("Профессия: " + proffesionRnd[counter] + " Стаж работы: " + character.expWork(age));
                    f.WriteLine("Пол: " + gender[rnd.Next(0, 2)]);
                    f.WriteLine("Возраст: " + age);
                    f.WriteLine("Хобби: " + hobbyRnd[counter] + ". Стаж хобби: " + character.expHobby(age) + " лет");
                }
                //характеристики для всех
                f.WriteLine(character.Health(healthRnd[counter]));
                f.WriteLine("Фобия: " + phobiaRnd[counter]);
                f.WriteLine("Человеческая черта: " + HTR[counter]);
                f.WriteLine("Багаж: " + luggageRnd[counter]);
                f.WriteLine("Телосложение: " + character.RashetIMT(healthRnd[counter]));
                f.WriteLine("Дополнительная информация: " + addInfoRnd[counter]);
                f.WriteLine("Карточка действий 1: " + kartochka1Rnd[counter]);
                f.WriteLine("Карточка действий 2: " + kartochka1Rnd[kart]);
                f.Close(); //закрываем документ
                character.VragDrug(filepath);//вызываем метод для генерация карточек врага/друга
            }
            richTextBox1.Text = character.Disaster(); //записываем в richTextBox1 выход метода Disaster
            richTextBox2.Text = character.Bunker(oborudovano, estInBunker); //записываем в richTextBox2 выход метода Bunker с входными параметрами
        }

        //метод для изменения определённой хар-ки определённого игрока при нажатии на кнопку
        private void button2_Click(object sender, EventArgs e)
        {
            string numberplayer = textBox2.Text; //считываем с textBox2 номер игрока
            string sItem = System.Convert.ToString(listBox1.SelectedItem); //считывваем выбранное значение
            character.OneSwapCharacter(sItem, Put, numberplayer); //передаём значение, путь к файлу, номер игрока
        }

        //перегенеровать катастрофу
        private void button12_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = character.Disaster();//записываем в richTextBox1 выход метода Disaster
        }

        //перегенировать бункер
        private void button13_Click(object sender, EventArgs e)
        {
            //записываем в richTextBox2 выход метода Bunker с входными параметрами
            richTextBox2.Text = character.Bunker(character.ShuffleAndRandom(oborudovano.Length, oborudovano), character.ShuffleAndRandom(estInBunker.Length, estInBunker));
        }

        //Смена всех хар-к
        private void button5_Click(object sender, EventArgs e)
        {
            string sItem = System.Convert.ToString(listBox2.SelectedItem);//считывваем выбранное значение
            character.AllSwapCharacter(sItem, Put, kolvo); //передаём значение, путь к папке, кол-во игроков
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void  button3_Click(object sender, EventArgs e)
        {
           // System.Diagnostics.Process.Start(@"C:\Users\User\source\repos\Bunker3.1\bin\Release\Bunker3.1.exe");
        }


    }
}
