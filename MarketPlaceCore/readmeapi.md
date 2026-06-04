# مستندات REST API پروژه MarketPlaceCore

این سند شامل جزئیات تمام نقاط انتهایی (Endpoints) موجود در سیستم MarketPlaceCore است. تمام خروجی ها با ساختار JSON استاندارد ارسال می شوند.

## احراز هویت (Authentication)

### ۱. ورود با نام کاربری و رمز عبور
- **آدرس:** /api/Account/login
- **متد:** POST
- **بدنه درخواست:**
```json
{
  "username": "admin",
  "password": "admin123"
}
```

### ۲. ورود سریع (OTP)
- **مرحله اول (درخواست کد):** POST به /api/Account/otp-request با بدنه "09120000000"
- **مرحله دوم (تایید کد):** POST به /api/Account/otp-login با بدنه:
```json
{
  "phoneNumber": "09120000000",
  "code": "12345"
}
```

## محصولات و تنوع ها (Products & Variants)

### ۱. دریافت لیست محصولات
- **آدرس:** /api/Product
- **متد:** GET

### ۲. دریافت جزئیات کامل محصول همراه با تنوع ها و نظرات
- **آدرس:** /api/Product/{id}
- **متد:** GET
- **ساختار پاسخ:** شامل آرایه ای از Variants (قیمت و موجودی مستقل) و Reviews (نظرات کاربران).

## سفارشات و کد تخفیف (Orders & Coupons)

### ۱. ثبت سفارش نهایی
- **آدرس:** /api/Order
- **متد:** POST
- **هدر:** Authorization: Bearer {token}
- **بدنه درخواست (Enterprise Structure):**
```json
{
  "items": [
    {
      "productId": 1,
      "productVariantId": 10,
      "quantity": 2
    }
  ],
  "couponCode": "OFF2024"
}
```

## نظرات و بررسی ها (Reviews)

### ۱. ثبت نظر جدید
- **آدرس:** /api/Review
- **متد:** POST
- **نکته:** سیستم به صورت خودکار بررسی می کند که آیا کاربر واقعاً خریدار محصول بوده است یا خیر تا تگ Verified Buyer را اعمال کند.

## لیست علاقه مندی ها (Wishlist)

### ۱. افزودن به لیست
- **آدرس:** /api/Wishlist/add/{productId}
- **متد:** POST

### ۲. اطلاع رسانی موجودی
- **آدرس:** /api/Wishlist/alert/{productId}
- **متد:** POST
