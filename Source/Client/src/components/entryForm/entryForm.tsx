import React from 'react';
import EntryFormClient from "@/components/entryForm/client/entryFormClient";
import { getUserId } from "@/lib/serversideUser";
import { getActivities } from "@/lib/backendApi";
import {
  Card,
  CardContent,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";


const EntryForm = async () => {
  try {
    const userId = await getUserId();
    const activities = await getActivities(userId);

    return (
      <Card>
        <CardHeader>
          <CardTitle>Enter Entry</CardTitle>
        </CardHeader>
        <CardContent className="max-w-md">
          <EntryFormClient selection={activities} />
        </CardContent>
      </Card>
    );
  } catch (error) {
    return <div>Failed to load activities</div>;
  }
};

export default EntryForm;