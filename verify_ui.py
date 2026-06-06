import asyncio
from playwright.async_api import async_playwright

async def run():
    async with async_playwright() as p:
        browser = await p.chromium.launch()
        page = await browser.new_page()

        # 1. Home Page
        await page.goto("http://localhost:5294")
        await page.screenshot(path="MarketPlaceCore/screenshots/main_home.png")
        print("Home page captured")

        # 2. Products Page
        await page.goto("http://localhost:5294/Products")
        await page.screenshot(path="MarketPlaceCore/screenshots/product_archive.png")
        print("Products page captured")

        # 3. Product Details
        await page.goto("http://localhost:5294/ProductDetails/1")
        await page.screenshot(path="MarketPlaceCore/screenshots/product_details.png")
        print("Details page captured")

        # 4. Cart Page
        await page.goto("http://localhost:5294/Cart")
        await page.screenshot(path="MarketPlaceCore/screenshots/cart_page.png")
        print("Cart page captured")

        # 5. Admin Dashboard
        await page.goto("http://localhost:5294/Admin")
        await page.screenshot(path="MarketPlaceCore/screenshots/admin_dashboard.png")
        print("Admin page captured")

        await browser.close()

asyncio.run(run())
