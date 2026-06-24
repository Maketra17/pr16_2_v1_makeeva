using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace _16_1_maakeeva
{
    public partial class MainWindow : Window
    {
        private List<Student> allStudents = new List<Student>();//список всех студентов

        public MainWindow()
        {
            InitializeComponent();
            dgStudents.ItemsSource = allStudents;//показываем список в таблице
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            string fio = txtSurname.Text.Trim();
            if (string.IsNullOrEmpty(fio))
            {
                MessageBox.Show("Введите фамилию!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);//выводим ошибку
                return;
            }

            ComboBoxItem selectedGroupItem = cmbGroup.SelectedItem as ComboBoxItem;//берем выбранную группу
            string group = "Пр-22";//по умолч
            if (selectedGroupItem != null && selectedGroupItem.Content != null)
            {
                group = selectedGroupItem.Content.ToString();
            }

            ComboBoxItem selectedSpecialtyItem = cmbSpecialty.SelectedItem as ComboBoxItem;//берем специальность
            string specialty = "птпм";//по умолч
            if (selectedSpecialtyItem != null && selectedSpecialtyItem.Content != null)
            {
                specialty = selectedSpecialtyItem.Content.ToString();
            }
            string subj1 = txtSubject1.Text.Trim();
            if (string.IsNullOrEmpty(subj1)) subj1 = "птпм";

            string subj2 = txtSubject2.Text.Trim();
            if (string.IsNullOrEmpty(subj2)) subj2 = "рпм";

            string subj3 = txtSubject3.Text.Trim();
            if (string.IsNullOrEmpty(subj3)) subj3 = "уп";

            int gr1 = int.Parse((cmbGrade1.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "5");
            int gr2 = int.Parse((cmbGrade2.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "5");
            int gr3 = int.Parse((cmbGrade3.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "5");

            Student newStudent = new Student(fio, group, specialty, subj1, gr1, subj2, gr2, subj3, gr3);
            allStudents.Add(newStudent);
            txtSurname.Clear();
            cmbGroup.SelectedIndex = 0;
            cmbSpecialty.SelectedIndex = 0;

            ObnovTabl();
        }

        private void Search_TextChanged(object sender, EventArgs e)//когда меняется поиск
        {
            ObnovTabl();
        }

        private void Filter_Changed(object sender, SelectionChangedEventArgs e)//когда меняется фильтр
        {
            ObnovTabl();
        }

        private void Sort_Changed(object sender, SelectionChangedEventArgs e)//когда меняется сортировка
        {
            ObnovTabl();
        }

        private void ObnovTabl()//обновление таблицы
        {
            if (dgStudents == null)
            {
                return;
            }

            try
            {
                string searchText = txtSearch.Text.Trim();//берем текст поиска
                bool useRegex = chkRegex.IsChecked == true;//проверяем, включены ли регулярки

                string filterGroup = "Все группы";
                string sortType = "По фамилии";

                ComboBoxItem filterItem = cmbFilter.SelectedItem as ComboBoxItem;//берем выбранный фильтр
                if (filterItem != null && filterItem.Content != null)
                {
                    filterGroup = filterItem.Content.ToString();
                }

                ComboBoxItem sortItem = cmbSort.SelectedItem as ComboBoxItem;//берем выбранную сортировку
                if (sortItem != null && sortItem.Content != null)
                {
                    sortType = sortItem.Content.ToString();
                }

                IEnumerable<Student> result = allStudents.AsEnumerable();//берем всех студентов

                // Поиск
                if (!string.IsNullOrEmpty(searchText))//если есть текст для поиска
                {
                    if (useRegex)//если используем регулярки
                    {
                        try
                        {
                            Regex regex = new Regex(searchText, RegexOptions.IgnoreCase);//создаем регулярку
                            result = result.Where(s => regex.IsMatch(s.famil) || regex.IsMatch(s.Group) || regex.IsMatch(s.Specialty));//ищем
                        }
                        catch (ArgumentException)//если регулярка кривая
                        {
                            MessageBox.Show("Ошибка в регулярном выражении!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                    }
                    else//обычный поиск
                    {
                        string lowerSearch = searchText.ToLower();//приводим к нижнему регистру
                        result = result.Where(s => s.famil.ToLower().Contains(lowerSearch) ||
                                                   s.Group.ToLower().Contains(lowerSearch) ||
                                                   s.Specialty.ToLower().Contains(lowerSearch));
                    }
                }

                // Фильтр по группе
                if (filterGroup != "Все группы")
                {
                    result = result.Where(s => s.Group == filterGroup);
                }

                // Сортировка
                if (sortType == "По фамилии")
                {
                    result = result.OrderBy(s => s.famil);
                }
                else if (sortType == "По группе")
                {
                    result = result.OrderBy(s => s.Group).ThenBy(s => s.famil);
                }
                else if (sortType == "По специальности")
                {
                    result = result.OrderBy(s => s.Specialty).ThenBy(s => s.famil);
                }

                List<Student> resultList = result.ToList();
                dgStudents.ItemsSource = resultList;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка при обновлении таблицы: {ex.Message}", "Ошибка");
                dgStudents.ItemsSource = allStudents;
            }
        }

        private void Delete_Selected(object sender, RoutedEventArgs e)//кнопка удаления
        {
            if (dgStudents.SelectedItem == null)
            {
                MessageBox.Show("Выберите студента для удаления!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            Student selected = dgStudents.SelectedItem as Student;//берем выбранного студента
            if (selected != null)
            {
                allStudents.Remove(selected);
                ObnovTabl();
            }
        }

        private void Clear_List(object sender, RoutedEventArgs e)//кнопка очистки
        {
            if (allStudents.Count == 0)
            {
                return;
            }
            allStudents.Clear();
            ObnovTabl();
        }
    }
}