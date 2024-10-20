export function createResponse(message: string, status: number): Response {
  return new Response(JSON.stringify({ message, status }), { status });
}
