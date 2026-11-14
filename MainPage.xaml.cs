using System.Collections.ObjectModel;
using System.Text.Json;
using ToDo2.Model;
using MongoDB.Driver;
using MongoDB.Bson;


namespace ToDo2
{
    public partial class MainPage : ContentPage
    {
        private readonly IMongoCollection<TaskItem> _taskColletion;
        public ObservableCollection<TaskItem> TaskList { get; set; } = new ObservableCollection<TaskItem>();

        public MainPage()
        {
            InitializeComponent();
            LoadTasks();
            taskListView.ItemsSource = TaskList;

            var client = new MongoClient("mongodb+srv://italosantoscont_db_user:<db_password>@cluster0.dvpumkc.mongodb.net/");
           
        }

        private void OnAddTaskClicked(object? sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(taskEntry.Text))
            {

                TaskItem newTask = new TaskItem { TaskName = taskEntry.Text };

                TaskList.Add(newTask);
                _taskColletion.InsertOne(newTask);
                SaveTasks();
                taskEntry.Text = string.Empty;
            }
        }

        private void OnCompleteTaskCliked (object ? sender, EventArgs e)
        {
            var button = sender as Button;
            var task = button.BindingContext as TaskItem;
            if (task != null)
            {
                task.TaskName += " (Concluida) ";
                _taskColletion.ReplaceOne(t => t.Id == task.Id, task); 
                DisplayAlert("Tarefa finalizada", "A tarefa " + task.TaskName, "Ok");
            }
        }

        private void OnDeleteTaskClicked(object sender, EventArgs e)
        {
            var button = sender as Button;

            button.TextColor = Colors.White;

            var task = button.BindingContext as TaskItem;

            if (task != null)
            {
                TaskList.Remove(task);

                _taskColletion.DeleteOne(t => t.Id == task.Id);
            }
        }
        private void SaveTasks()
        {
            var tasksJson = JsonSerializer.Serialize(TaskList);
            Preferences.Set("tasks", tasksJson);
        }
        private void LoadTasks()
        {
            var tasksFromDb = _taskColletion.Find(new BsonDocument()).ToList();

            TaskList = new ObservableCollection<TaskItem>(tasksFromDb);

            taskListView.ItemsSource = TaskList;
        }
    }

        
    
}

