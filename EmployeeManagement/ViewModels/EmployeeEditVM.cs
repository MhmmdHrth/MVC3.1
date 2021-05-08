namespace EmployeeManagement.ViewModels
{
    public class EmployeeEditVM : EmployeeCreateViewModel
    {
        public int Id { get; set; }

        public string ExistingPhotoPath { get; set; }
    }
}