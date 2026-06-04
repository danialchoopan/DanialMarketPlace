# راهنمای اتصال اپلیکیشن اندروید به MarketPlaceCore

این راهنما برای توسعه‌دهندگان موبایل جهت تعامل با API فروشگاه تهیه شده است.

## مدیریت احراز هویت

پس از دریافت توکن از Endpoint ورود، باید آن را در SharedPreferences یا EncryptedSharedPreferences ذخیره کنید و در تمامی درخواست‌های بعدی در هدر قرار دهید.

### نمونه استفاده در Retrofit
```kotlin
interface ApiService {
    @POST("api/Account/login")
    suspend fun login(@Body request: LoginRequest): Response<LoginResponse>

    @GET("api/Product")
    suspend fun getProducts(@Header("Authorization") token: String): List<ProductDto>
}
```

## فرآیند ثبت سفارش

۱. ابتدا لیست محصولات را دریافت کرده و در UI نمایش دهید.
۲. سبد خرید را در حافظه محلی گوشی (مانند Room Database) مدیریت کنید.
۳. هنگام نهایی‌سازی، لیست IDها و تعداد را در قالب ساختار JSON زیر به سرور ارسال کنید:

```json
{
  "items": [
    {"productId": 1, "quantity": 1},
    {"productId": 5, "quantity": 3}
  ]
}
```

## نکات مهم
- تمام قیمت‌ها به واحد ریال/تومان (بسته به داده‌های Seed) هندل می‌شوند.
- در صورت دریافت خطای 401، کاربر باید مجدداً وارد سیستم شود.
- برای نمایش تصاویر از ImageUrl موجود در مدل محصول استفاده کنید.
