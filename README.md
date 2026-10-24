# Ocelot API Gateway Integration Activity

## Part C — Complete Test Matrix Table

| Request (via http://localhost:5000) | Token | Expected Status | Dahilan / Behavior |
|---|---|---|---|
| `GET /gateway/products` | None | `200 OK` | Public route; pinapayagan ang pagbasa nang walang token. |
| `GET /gateway/products/1` | None | `200 OK` | Public route; pinapayagan ang pagbasa ng partikular na item. |
| `POST /gateway/auth/login` | None | `200 OK + Token` | Magbabalik ng valid JWT Bearer token kung tama ang credentials. |
| `POST /gateway/products` | None | `401 Unauthorized` | Harang agad sa Gateway pa lang dahil walang GatewayKey token. |
| `POST /gateway/products` | Valid | `201 Created` / `200 OK` | Naka-pasa ang token sa Gateway kaya nai-forward sa Products service. |
| `GET /gateway/orders` | None | `401 Unauthorized` | Harang sa Gateway dahil kailangan ng token para sa orders. |
| `GET /gateway/orders` | Valid | `200 OK` | Naka-pasa sa Gateway at naipakita ang orders mula sa service. |
| `POST /gateway/auth/login` (x6 mabilis) | None | `429 Too Many Requests` | Na-trigger ang Rate Limiting sa Gateway (max 5 requests per 10s). |

---

## Questions & Answers

**Q: Which requests does the gateway reject before they reach a service, and why?**

**Answer:**
1. **Unauthenticated Writes and Protected Routes (`POST /gateway/products` without token & `GET /gateway/orders` without token):** 
   Rejected with a `401 Unauthorized` status code directly at the Ocelot Gateway because they are configured with `AuthenticationOptions` (`AuthenticationProviderKey: "GatewayKey"`). The gateway validates the JWT token before routing the request downstream.
2. **Excessive Requests (`POST /gateway/auth/login` on the 6th rapid attempt):** 
   Rejected with a `429 Too Many Requests` status code because of the Ocelot Rate Limiting policy (`Limit: 5` within a `10s` period). The gateway throttles incoming traffic to protect the downstream service from brute-force or abuse.