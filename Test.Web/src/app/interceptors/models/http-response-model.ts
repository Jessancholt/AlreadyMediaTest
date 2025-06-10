export interface HttpResponseMessage {
  code?: HttpResponseCode;
  title?: string;
  message?: string;
}

export enum HttpResponseCode {
    badRequest = 400,
    notFound = 404,
    serverError = 500,
}