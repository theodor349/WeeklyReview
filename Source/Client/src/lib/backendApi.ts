import { getEnvVariable } from "@/lib/env";

export async function postEntry(body: EntryRequestBody): Promise<BackendApiResponse> {
  return post(`/api/v1/entry`, body);
}

export async function getActivities(userId: string) {
  const endpoint = `/api/v1/user/${userId}/activities`;
  const response = await get(endpoint);

  if (response.status === 204) {
    return [];
  }

  const data = await response.json();
  return data.map((item: Activity) => item.name);
}

/////////////////////////////////////////
//             API Helpers             //
/////////////////////////////////////////

async function post(endpoint: string, body: any, headers: Record<string, string> = {}): Promise<BackendApiResponse> {
  try {
    const baseUrl = getEnvVariable("BACKEND_URL");
    const url = `${baseUrl}${endpoint}`;

    var functionsKey = getEnvVariable("FUNCTIONS_KEY");
    headers["x-functions-key"] = functionsKey;

    const response = await fetch(url, {
      method: "POST",
      body: JSON.stringify(body),
      headers: {
        "Content-Type": "application/json",
        ...headers,
      },
    });

    if (!response.ok) {
      console.error(`Request to ${url} failed with status: ${response.statusText}`);
      return { success: false, status: response.status, message: "Failed to add entry" };
    }

    return { success: true, status: response.status, message: "Entry added" };
  } catch (error) {
    console.error("Error making request:", error);
    return { success: false, status: 500, message: "Internal server error" };
  }
}

async function get(endpoint: string, headers: Record<string, string> = {}): Promise<Response> {
  try {
    const baseUrl = getEnvVariable("BACKEND_URL");
    const url = `${baseUrl}${endpoint}`;

    var functionsKey = getEnvVariable("FUNCTIONS_KEY");
    headers["x-functions-key"] = functionsKey;

    const response = await fetch(url, {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
        ...headers,
      },
    });

    if (!response.ok && response.status !== 204) {
      console.error(`Request to ${url} failed with status: ${response.statusText}`);
      throw new Error(`Failed to fetch data from ${url}`);
    }

    return response;
  } catch (error) {
    console.error("Error making request:", error);
    throw error;
  }
}
