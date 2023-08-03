using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeeklyReview.Shared.Models;
using WeeklyReview.Shared.Services;
using Xunit;

namespace WeeklyReview.Shared.Tests.Services
{
    public class ActivityRollBackServiceTests
    {
        private readonly ActivityRollBackService _sut;

        private SortedDictionary<int, CategoryModel> _categories;
        private SortedDictionary<int, ActivityModel> _activities;
        private SortedDictionary<int, EntryModel> _entries;
        private SortedDictionary<int, ActivityChangeModel> _changes;
        private DateTime _dt;

        public ActivityRollBackServiceTests()
        {
            IDataService dataservice = null;
            _sut = new ActivityRollBackService(dataservice);

            _categories = new SortedDictionary<int, CategoryModel>()
            {
                { 1, new CategoryModel(1, "Food", 1, Color.Yellow) },
                { 2, new CategoryModel(2, "Sports", 2, Color.Green) },
                { 3, new CategoryModel(3, "Shopping", 2, Color.Blue) },
                { 4, new CategoryModel(4, "Television", 2, Color.Red) },
            };
            _activities = new SortedDictionary<int, ActivityModel>()
            {
                { 1, new ActivityModel(1, "Dinner", false, _categories[1]) },
                { 2, new ActivityModel(2, "Lunch", false, _categories[1]) },
                { 3, new ActivityModel(3, "Bike", true, _categories[2]) },
                { 4, new ActivityModel(4, "Shopping", false, _categories[3]) },
                { 5, new ActivityModel(5, "Swim", false, _categories[2]) },
                { 6, new ActivityModel(6, "Youtube", false, _categories[4]) },
                { 7, new ActivityModel(7, "TV", false, _categories[4]) },
            };
            _dt = new DateTime(2023, 8, 1, 10, 0, 0);
            _entries = new SortedDictionary<int, EntryModel>()
            {
                { 1, new EntryModel(1, _dt, _dt.AddHours(1), new List<ActivityModel>() { _activities[1], _activities[6] }, false) },
                { 2, new EntryModel(2, _dt.AddHours(1), _dt.AddHours(2), new List<ActivityModel>() { _activities[2] }, false) },
                { 3, new EntryModel(3, _dt.AddHours(2), _dt.AddHours(3), new List<ActivityModel>() { _activities[3], _activities[4] }, true) },
                { 4, new EntryModel(4, _dt.AddHours(3), _dt.AddHours(4), new List<ActivityModel>() { _activities[3] }, true) },
                { 5, new EntryModel(5, _dt.AddHours(4), _dt.AddHours(5), new List<ActivityModel>() { _activities[5], _activities[7] }, false) },
                { 6, new EntryModel(6, _dt.AddHours(2), _dt.AddHours(3), new List<ActivityModel>() { _activities[5], _activities[4] }, true) },
                { 7, new EntryModel(7, _dt.AddHours(3), _dt.AddHours(4), new List<ActivityModel>() { _activities[5] }, true) },
                { 8, new EntryModel(8, _dt.AddHours(2), _dt.AddHours(3), new List<ActivityModel>() { _activities[1], _activities[4] }, false) },
                { 9, new EntryModel(9, _dt.AddHours(3), _dt.AddHours(4), new List<ActivityModel>() { _activities[5], _activities[2] }, false) },
            };
            _changes = new SortedDictionary<int, ActivityChangeModel>()
            {
                { 1, new ActivityChangeModel(1, 3, 5, _dt.AddHours(6)) },
            };
        }


        // Case: No new Entry has been added after the change
        [Fact]
        public void Rolback_NoNewEntry_AddOldEntry()
        {
            // Arrange

            // Act

            // Assert
            var newAct = _entries[9];
            Assert.False(_activities[3].Deleted);
            Assert.True(_entries[6].Deleted);
            Assert.Equal(_dt.AddHours(2), newAct.StartTime);
            Assert.Equal(_dt.AddHours(3), newAct.EndTime);
            Assert.Equal(2, newAct.Activities.Count());
            Assert.Contains(newAct.Activities, x => x.Id == 3);
            Assert.Contains(newAct.Activities, x => x.Id == 4);
            Assert.Equal(0, _changes.Count());
        }

        // Case: A new Entry has been added, but where the changed activity is used again
        [Fact]
        public void Rolback_NewEntryWithSameOldActivity_AddOldEntry()
        {
            // Arrange
            _entries[7].Deleted = true;
            _entries.Add(9, new EntryModel(9, _dt.AddHours(3), _dt.AddHours(4), new List<ActivityModel>() { _activities[5], _activities[2] }, false));

            // Act

            // Assert
            var newAct = _entries[10];
            Assert.False(_activities[3].Deleted);
            Assert.True(_entries[9].Deleted);
            Assert.Equal(_dt.AddHours(3), newAct.StartTime);
            Assert.Equal(_dt.AddHours(4), newAct.EndTime);
            Assert.Equal(2, newAct.Activities.Count());
            Assert.Contains(newAct.Activities, x => x.Id == 3);
            Assert.Contains(newAct.Activities, x => x.Id == 2);
            Assert.Equal(0, _changes.Count());
        }

        // Case: A new Entry has been added, but where the changed activity is not used again
        [Fact]
        public void Rolback_NewEntryWithoutSameOldActivity_DoNotAddNewEntry()
        {
            // Arrange

            // Act

            // Assert
            Assert.False(_activities[3].Deleted);
            Assert.False(_entries[8].Deleted);
            Assert.Equal(9, _entries.Count());
            Assert.Equal(0, _changes.Count());
        }

        // Case: A new Entry has been added, but where the changed activity is not used again
        [Fact]
        public void Rolback_NewEntryWithAndWithoutSameOldActivity_DoNotAddNewEntry()
        {
            // Arrange
            _entries[7].Deleted = true;
            _entries.Add(9, new EntryModel(9, _dt.AddHours(3), _dt.AddHours(4), new List<ActivityModel>() { _activities[5], _activities[2] }, true));
            _entries.Add(10, new EntryModel(10, _dt.AddHours(3), _dt.AddHours(4), new List<ActivityModel>() { _activities[1] }, true));
            _entries.Add(11, new EntryModel(10, _dt.AddHours(3), _dt.AddHours(4), new List<ActivityModel>() { _activities[5] }, false));

            // Act

            // Assert
            Assert.False(_activities[3].Deleted);
            Assert.False(_entries[11].Deleted);
            Assert.Equal(11, _entries.Count());
            Assert.Equal(0, _changes.Count());
        }
    }
}
