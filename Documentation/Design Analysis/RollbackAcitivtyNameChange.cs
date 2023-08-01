var oldActivity = new Activity();	// Renamed from
var newActivity = new Activity();	// To this
var times = _api.GetEntryTimesWhereActivityWasUsed(oldActivity, getDeleted: true);
foreach (var time in times)
{
	var entries = _api.GetEntriesAt(time);
	entries.Sort(x => x.Id, ascending: true);
	var oldEntry = entries.Last(x => x.Id = oldActivity.Id);
	if(AllEntriesAfterInclude(newEntry, newActivity) == false)
		return;
	var newestEntry = entries.Last();
	var newEntry = new Entry();
	foreach (var activity in newestEntry.Activities){
		if(activity.Id == newActivity)
			newEntry.AddEntry(oldActivity);
		else 
			newEntry.AddEntry(activity);
	}
	_api.AddEntry(newEntry);
}