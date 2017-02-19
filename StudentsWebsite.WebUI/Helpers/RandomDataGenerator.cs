﻿using System;

namespace StudentsWebsite.WebUI.Helpers
{
    public static class RandomDataGenerator
    {
        private static Random rnd = new Random();

        private static string symbols = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890_";

        private static string[] maNames =
        {
            "Андрей", "Артём", "Алексей", "Анатолий", "Александр", "Антон", "Аркадий", "Борис", "Богдан", "Владислав",
            "Виктор", "Всеволод",
            "Владимир", "Виталий", "Валерий", "Григорий", "Георгий",
            "Дмитрий", "Данил", "Евгений", "Иван", "Игорь", "Константин", "Кирилл", "Леонид", "Лев", "Максим", "Михаил",
            "Николай", "Никита", "Олег", "Пётр",
            "Павел", "Роман", "Руслан", "Степан", "Семён", "Станислав", "Тимофей", "Фёдор", "Эдуард", "Юрий", "Ярослав"
        };

        private static string[] feNames =
        {
            "Анастасия", "Альбина", "Анна", "Александра", "Алина", "Виктория", "Варвара", "Валерия", "Галина",
            "Дарья", "Диана", "Екатерина", "Елена", "Елизавета", "Жанна", "Зоя", "Ирина", "Кристина", "Ксения",
            "Лидия", "Людмила", "Любовь", "Марина", "Мария", "Нина", "Наталья", "Надежда", "Ольга", "Олеся",
            "Станислава",
            "Светлана", "Татьяна", "Ульяна"
        };

        private static string[] maLastNames =
        {
            "Абашин", "Алтуфьев", "Аганаев", "Анисимов", "Белоконь", "Баранов", "Васильев", "Воронов",
            "Воробьев", "Грибоедов", "Гагарин", "Гребеников", "Гоголев", "Долганов", "Данильчук",
            "Еничев", "Ефимов", "Жарков", "Захаров", "Зыбарев", "Игнатьев", "Иксанов", "Иванов",
            "Камнев", "Краснов", "Кокин", "Кузнецов", "Ласточкин", "Литвинов", "Леонтьев", "Матушкин",
            "Митриков", "Марков", "Мороз", "Новиков", "Никифоров", "Овчинников", "Проводников",
            "Пасынков", "Помешкин", "Родников", "Расстегаев", "Романов", "Соболев", "Султанов",
            "Тихонов", "Тополев", "Тарасов", "Улиткин", "Федоров", "Федосеев", "Хомяков", "Харитонов",
            "Хасанов", "Цой", "Черных", "Чащин", "Шашин", "Шелковников", "Юсупов", "Юрин", "Ястребов"
        };

        private static string[] feLastNames =
        {
            "Абашина", "Алтуфьева", "Аганаева", "Анисимова", "Белоконь", "Баранова", "Васильева", "Воронова",
            "Воробьева", "Грибоедова", "Гагарина", "Гребеникова", "Гоголева", "Долганова", "Данильчук",
            "Еничева", "Ефимова", "Жаркова", "Захарова", "Зыбарева", "Игнатьева", "Иксанова", "Иванова",
            "Камнева", "Краснова", "Кокина", "Кузнецова", "Ласточкина", "Литвинова", "Леонтьева", "Матушкина",
            "Митрикова", "Маркова", "Мороз", "Новикова", "Никифорова", "Овчинникова", "Проводникова",
            "Пасынкова", "Помешкина", "Родникова", "Расстегаева", "Романова", "Соболева", "Султанова",
            "Тихонова", "Тополева", "Тарасова", "Улиткина", "Федорова", "Федосеева", "Хомякова", "Харитонова",
            "Хасанова", "Цой", "Черных", "Чащина", "Шашина", "Шелковникова", "Юсупова", "Юрина", "Ястребова"
        };

        private static string[] subjects =
        {
            "Математический анализ", "Линейная алгебра", "Аналитическая геометрия", "Теория автоматики и управления",
            "Сигналы и системы",
            "Физика", "Спецглавы физики", "Термодинамика", "Сопротивление материалов", "Физика плазмы",
            "Квантовая физика",
            "Общая химия", "Органическая химия", "Химия расплавов",
            "Электротехника", "Переходные процессы в электронных устройствах", "Электроника", "Микроконтроллеры",
            "Программирование C++", "Программирование Java", "Программирование C#", "Низкоуровневое программирование",
            "История", "Отечественная история", "Экономика", "Правоведение",
            "Английский язык", "Немецкий язык", "Китайский язык", "Японский язык", "Корейский язык",
            "Стандартизация", "Метрология"
        };

        private static int _subject_index = 0;
        public static void RandomSeed(int seed)
        {
            rnd = new Random(seed);
        }

        public static string RandomSubject()
        {
            return subjects[rnd.Next(subjects.Length - 1)];
        }
        public static string NextSubject()
        {
            if (_subject_index >= subjects.Length)
                _subject_index = 0;

            return subjects[_subject_index++];
        }
        public static string RandomString(int length)
        {
            string pass = "";

            for (int i = 0; i < length; i++)
            {
                pass += symbols[rnd.Next(symbols.Length - 1)];
            }

            return pass;
        }

        public static string RandomEmail()
        {
            return Guid.NewGuid() + "@mail.com";
        }

        public static void RandomName(out string firstName, out string lastName)
        {
            if (rnd.NextDouble() < 0.5)
            {
                firstName = maNames[rnd.Next(maNames.Length - 1)];
                lastName = maLastNames[rnd.Next(maLastNames.Length - 1)];
            }
            else
            {
                firstName = feNames[rnd.Next(feNames.Length - 1)];
                lastName = feLastNames[rnd.Next(feLastNames.Length - 1)];
            }
        }


    }
}