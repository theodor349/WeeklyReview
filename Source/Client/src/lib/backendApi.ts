import { getEnvVariable } from "@/lib/env";

export async function postRequest(endpoint: string, body: any, headers: Record<string, string> = {}): Promise<BackendApiResponse> {
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
