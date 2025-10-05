#!/usr/bin/env pwsh

# Test both authentication methods to see which one works
# Run with: pwsh TestBothAuthMethods.ps1

Write-Host "🔍 Testing Both Authentication Methods..." -ForegroundColor Cyan
Write-Host "===========================================" -ForegroundColor Cyan

# Using your actual credentials from user secrets
$haloAccount = "panoramicdlsandbox"
$clientId = "8f7c282e-634a-42dc-8c85-23b35b513127"
$clientSecret = "30546cfe-f657-4472-97aa-b9331924bbc8-06a09701-efda-478f-b73b-97c3539e9c0c"

$baseUrl = "https://$haloAccount.halopsa.com"
Write-Host "🌐 Base URL: $baseUrl" -ForegroundColor Yellow
Write-Host "🔑 Client ID: $clientId" -ForegroundColor Yellow
Write-Host "🔐 Client Secret: $($clientSecret.Substring(0,20))..." -ForegroundColor Yellow
Write-Host ""

# Test 1: Try the original client_credentials method (what we were using before)
Write-Host "🔐 Test 1: Trying client_credentials grant..." -ForegroundColor Green

$body1 = @{
    grant_type = "client_credentials"
    client_id = $clientId
    client_secret = $clientSecret
    scope = "all"
}

$authEndpoints = @("/auth/token", "/auth/token?tenant=Halo")

foreach ($endpoint in $authEndpoints) {
    try {
        $authUrl = "$baseUrl$endpoint"
        Write-Host "   Testing: $authUrl" -ForegroundColor White
        
        $authResponse = Invoke-WebRequest -Uri $authUrl -Method POST -Body $body1 -ContentType "application/x-www-form-urlencoded" -TimeoutSec 10
        
        Write-Host "   Status: $($authResponse.StatusCode)" -ForegroundColor White
        Write-Host "   Content Length: $($authResponse.Content.Length)" -ForegroundColor White
        
        if ($authResponse.StatusCode -eq 200) {
            # Try to parse as JSON
            try {
                $jsonResponse = $authResponse.Content | ConvertFrom-Json
                if ($jsonResponse.access_token) {
                    $token = $jsonResponse.access_token
                    Write-Host "   ✅ SUCCESS! Got access token with client_credentials!" -ForegroundColor Green
                    Write-Host "   🎯 Token: $($token.Substring(0,[Math]::Min(20,$token.Length)))..." -ForegroundColor Green
                    
                    # Test the API with this token
                    $headers = @{
                        Authorization = "Bearer $token"
                        'Content-Type' = 'application/json'
                    }
                    
                    $apiUrl = "$baseUrl/api/tickets"
                    Write-Host "   Testing API: $apiUrl" -ForegroundColor White
                    
                    try {
                        $apiResponse = Invoke-WebRequest -Uri $apiUrl -Method GET -Headers $headers -TimeoutSec 10
                        Write-Host "   ✅ API call successful! Status: $($apiResponse.StatusCode)" -ForegroundColor Green
                        Write-Host "   Response length: $($apiResponse.Content.Length)" -ForegroundColor White
                        Write-Host ""
                        Write-Host "🎉 RESULT: client_credentials method works perfectly!" -ForegroundColor Green
                        Write-Host "   Your original implementation was correct!" -ForegroundColor Green
                        exit 0
                    }
                    catch {
                        Write-Host "   ❌ API call failed: $($_.Exception.Message)" -ForegroundColor Red
                    }
                }
            }
            catch {
                Write-Host "   ⚠️  Response is not valid JSON" -ForegroundColor Yellow
                Write-Host "   Content: $($authResponse.Content.Substring(0,[Math]::Min(300,$authResponse.Content.Length)))" -ForegroundColor Yellow
            }
        }
        else {
            Write-Host "   ❌ Auth failed: $($authResponse.StatusCode)" -ForegroundColor Red
        }
    }
    catch {
        Write-Host "   ❌ Exception: $($_.Exception.Message)" -ForegroundColor Red
    }
}

Write-Host ""
Write-Host "📝 Test 1 Result: client_credentials method failed" -ForegroundColor Yellow
Write-Host ""

# Test 2: Show what password method would look like (but we don't have username/password)
Write-Host "🔐 Test 2: Password grant method (requires username/password)..." -ForegroundColor Green
Write-Host "   This method requires actual Halo user credentials" -ForegroundColor Yellow
Write-Host "   Format would be:" -ForegroundColor Yellow
Write-Host "   {" -ForegroundColor Gray
Write-Host "     grant_type: 'password'," -ForegroundColor Gray
Write-Host "     client_id: '$clientId'," -ForegroundColor Gray
Write-Host "     username: 'your-halo-username'," -ForegroundColor Gray
Write-Host "     password: 'your-halo-password'," -ForegroundColor Gray
Write-Host "     scope: 'all'" -ForegroundColor Gray
Write-Host "   }" -ForegroundColor Gray
Write-Host ""

# Test 3: Try the alternative approach - maybe it's OAuth2 but with different parameters
Write-Host "🔐 Test 3: Alternative OAuth2 approaches..." -ForegroundColor Green

# Try with different parameter names
$body3 = @{
    grant_type = "client_credentials"
    client_id = $clientId
    client_secret = $clientSecret
    resource = "api"
}

try {
    $authUrl = "$baseUrl/auth/token"
    Write-Host "   Testing with 'resource' parameter: $authUrl" -ForegroundColor White
    
    $authResponse = Invoke-WebRequest -Uri $authUrl -Method POST -Body $body3 -ContentType "application/x-www-form-urlencoded" -TimeoutSec 10
    
    Write-Host "   Status: $($authResponse.StatusCode)" -ForegroundColor White
    
    if ($authResponse.StatusCode -eq 200) {
        try {
            $jsonResponse = $authResponse.Content | ConvertFrom-Json
            if ($jsonResponse.access_token) {
                Write-Host "   ✅ SUCCESS with alternative parameters!" -ForegroundColor Green
                exit 0
            }
        }
        catch {
            Write-Host "   Response: $($authResponse.Content.Substring(0,[Math]::Min(200,$authResponse.Content.Length)))" -ForegroundColor Yellow
        }
    }
}
catch {
    Write-Host "   ❌ Alternative method failed: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host ""
Write-Host "💡 CONCLUSION:" -ForegroundColor Cyan
Write-Host "   None of the authentication methods worked with the provided credentials." -ForegroundColor White
Write-Host "   This could mean:" -ForegroundColor White
Write-Host "   1. The API requires actual username/password (as per documentation)" -ForegroundColor White
Write-Host "   2. The client_id/client_secret need to be registered differently" -ForegroundColor White
Write-Host "   3. There might be additional setup required in the Halo system" -ForegroundColor White
Write-Host "   4. The sandbox environment might have different requirements" -ForegroundColor White
exit 1