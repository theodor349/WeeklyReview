interface ApiResponse {
  message: string;
  status: number;
}

interface BackendApiResponse {
  success: boolean;
  status: number;
  message: string;
}

interface EntryRequestBody {
  userGuid: string;
  title: string;
  description: string;
}


interface Activity {
  name: string;
};