﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Информационная_система_для_больницы.Pages.General
{
    /// <summary>
    /// Логика взаимодействия для InfoPage.xaml
    /// </summary>
    public partial class InfoPage : Page
    {
        public InfoPage()
        {
            InitializeComponent();
            info.Text = "Информационная система для больницы предназначена для обеспечения хранения, накопления и\nпредоставления всей необходимой информации о\n сотрудниках, пациентах, показателях, препаратах, процедурах, кроватях, оформлениях,\nсостоянии пациентов, назначениях и расписании назначений.\r\n" + "Данная информационная система эксплуатируется в больнице.\r\nОсновными функциями информационной системы являются:\r\n- оперативный доступ к имеющимся данным;\r\n- ввод, хранение, изменение и удаление информации о сотрудниках, пациентах, препаратах,\nпроцедурах, кроватях, оформлениях, состоянии пациентов,\nназначениях и расписании назначений;\r\n- фильтрация информации;\r\n- сортировка информации по убыванию и возрастанию;\r\n- поиск оформлений;\r\n" + "Конечными пользователями информационной системы являются администраторы, регистраторы,\nврачи и медицинский персонал.";
            //info.Text = "Информационная система для больницы предназначена для обеспечения хранения, накопления и предоставления всей необходимой информации о сотрудниках, пациентах, показателях, препаратах, процедурах, кроватях, оформлениях, состоянии пациентов, назначениях и расписании назначений.\r\nДанная информационная система эксплуатируется в больнице.\r\nОсновными функциями информационной системы являются:\r\n- оперативный доступ к имеющимся данным;\r\n- ввод, хранение, изменение и удаление информации о сотрудниках, пациентах, препаратах, процедурах, кроватях, оформлениях, состоянии пациентов, назначениях и расписании назначений;\r\n- фильтрация информации;\r\n- сортировка информации по убыванию и возрастанию;\r\n- поиск оформлений;\r\nКонечными пользователями информационной системы являются администраторы, регистраторы, врачи и медицинский персонал.\r\n";
            credits.Text = "Руководитель: Кононова Ольга Александровна\n" + "Разработчик: Хорошев Дмитрий Романович ИР-41\n\n" + "Помощь в разработке тем:\n" + "Анталь Илья\n" + "Кутявин Михаил\n" + "Ражик Владислав\n" + "Рякин Роман\n" + "Кашина Анастасия";
        }
    }
}
