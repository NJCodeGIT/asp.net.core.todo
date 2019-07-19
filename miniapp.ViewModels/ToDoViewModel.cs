using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace miniapp.ViewModels
{
    public class ToDoViewModel: EntityBaseViewModel
    {
        private List<ToDoViewModel> _toDoViewModelList;
        public List<ToDoViewModel> ToDoViewModelList
        {
            get { return _toDoViewModelList ?? new List<ToDoViewModel>(); }
            set => _toDoViewModelList = value;
        }
        [DisplayName("Is Complete")]
        public bool Status { get; set; }
        [DisplayName("Due Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d/M/yyy}")]
        [DataType(DataType.Date)]
        public DateTime? DueDate { get; set; }
    }
}
