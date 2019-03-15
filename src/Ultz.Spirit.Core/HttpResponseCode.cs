// 
// HttpResponseCode.cs
// 
// Copyright (C) 2019 Ultz Limited
// 
// This software may be modified and distributed under the terms
// of the MIT license. See the LICENSE file for details.
// 

namespace Ultz.Spirit.Core
{
    public enum HttpResponseCode
    {
        // Informational
        Continue = 100,
        SwitchingProtocols = 101,
        Processing = 102,

        // Success
        Ok = 200,
        Created = 201,
        Accepted = 202,
        NonAuthorativeInformation = 203,
        NoContent = 204,
        ResetContent = 205,
        PartialContent = 206,
        MultiStatus = 207,
        AlreadyReported = 208,
        ImUsed = 226,

        // Redirection
        MovedPermanently = 301,
        Found = 302,
        SeeOther = 303,
        NotModified = 304,
        UseProxy = 305,
        SwitchProxy = 306,
        TemporaryRedirect = 307,
        PermanentRedirect = 308,

        // Client Error
        BadRequest = 400,
        Unauthorized = 401,
        PaymentRequired = 402,
        Forbidden = 403,
        NotFound = 404,
        MethodNotAllowed = 405,
        NotAcceptable = 406,
        ProxyAuthenticationRequired = 407,
        RequestTimeout = 408,
        Conflict = 409,
        Gone = 410,
        LengthRequired = 411,
        PreconditionFailed = 412,
        RequestEntityTooLarge = 413,
        RequestUriTooLong = 414,
        UnsupportedMediaType = 415,
        RequestedRangeNotSatisfiable = 416,
        ExpectationFailed = 417,
        ImATeapot = 418,
        AuthenticationTimeout = 419,
        MethodFailure = 420,
        UnprocessableEntity = 422,
        Locked = 423,
        FailedDependency = 424,
        UnorderedCollection = 425,
        UpgradeRequired = 426,
        PrecondittionRequired = 428,
        TooManyRequests = 429,
        RequestHeaderFieldsTooLarge = 431,
        LoginTimeout = 440,
        NoResponse = 444,
        RetryWith = 449,
        BlockedByWindowsParentalControls = 450,
        UnavailableForLegalReasons = 451,
        RequestHeaderTooLarge = 494,
        CertError = 495,
        NoCert = 496,
        HttpToHttps = 497,
        ClientClosedRequest = 499,

        // Server Errors
        InternalServerError = 500,
        NotImplemented = 501,
        BadGateway = 502,
        ServiceUnavailable = 503,
        GatewayTimeout = 504,
        HttpVersionNotSupported = 505,
        VariantAlsoNegotiates = 506,
        InsufficientStorage = 507,
        LoopDetected = 508,
        BandwidthLimitExceeded = 509,
        NotExtended = 510,
        NetworkAuthenticationRequired = 511,
        OriginError = 520,
        WebServerIsDown = 521,
        ConnectionTimedOut = 522,
        ProxyDeclinedRequest = 523,
        ATimeoutOccured = 524,
        NetworkReadTimeoutError = 598,
        NetworkRConnectTimeoutError = 599
    }
}
