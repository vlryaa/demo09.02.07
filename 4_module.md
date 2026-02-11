СОЗДАЙТЕ **ПОЛЬЗОВАТЕЛЬСКИЙ ЭЛЕМЕНТ УПРАВЛЕНИЯ** ПО ШАБЛОНУ С ЗАДАНИЯ И НАЗОВИТЕ ФАЙЛ UserControl, листинг представлен в файле UserControl1.cs

<img width="531" height="120" alt="Снимок экрана 2026-02-11 в 10 05 18" src="https://github.com/user-attachments/assets/937d0697-f8c5-4210-8009-b0ab382daa51" />

Результат представлен ниже:

<img width="1552" height="917" alt="Снимок экрана 2026-02-11 в 10 06 18" src="https://github.com/user-attachments/assets/a80cbf44-d6c0-43a1-8249-ba35ef25bd09" />

Добавим на вторую формы кнопку Заказы:

<img width="1552" height="917" alt="Снимок экрана 2026-02-11 в 10 33 47" src="https://github.com/user-attachments/assets/08599d3a-3b3c-467e-aa90-358f7e039d48" />

Создадим 4 форму(добавьте элементы соответсвующие скриншоту), листинг кода будет в файле Form4.cs

<img width="1552" height="917" alt="Снимок экрана 2026-02-11 в 10 47 08" src="https://github.com/user-attachments/assets/92381ee9-7ce1-428c-8341-518f38aab1db" />

Кликните 2 раза во 2 форме по кнопке ЗАКАЗЫ и добавьте следующий код:

            int role = 3; // По умолчанию гость

            if (label1.Text.Contains("Администратор")) role = 1;
            else if (label1.Text.Contains("Менеджер")) role = 2;

            // Открываем форму со списком заказов (Form4)
            Form4 f4 = new Form4(role);
            f4.Show();
<img width="1292" height="467" alt="Снимок экрана 2026-02-11 в 11 03 43" src="https://github.com/user-attachments/assets/a75d3074-fb98-40fe-a45a-39b11906a4fb" />

Далее создайте 5 форму(элементы и имена сделайте в соотвествие со скриншотом ниже), листинг находится в файле Form5.cs
