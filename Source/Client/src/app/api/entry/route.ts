import { postRequest } from "@/lib/backendApi";
import { createResponse } from "@/lib/response";
import { getUserId } from "@/lib/serversideUser";

export async function POST(req: Request): Promise<Response> {
  try {
    let body: EntryRequestBody;
    try {
      body = await req.json();
    } catch (error) {
      return createResponse("Invalid JSON format", 400);
    }

    body.userGuid = await getUserId();

    const result = await postRequest(`/api/v1/entry`, body);

    return createResponse(result.message, result.status);
  } catch (error) {
    console.error("Unexpected error in POST handler:", error);
    return createResponse("Internal server error", 500);
  }
}
