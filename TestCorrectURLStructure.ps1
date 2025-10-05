#!/usr/bin/env pwsh

# Test with the CORRECT URL structure from documentation
# Authorization Endpoint: https://panoramicdlsandbox.halopsa.com/auth
# Resource Server: https://panoramicdlsandbox.halopsa.com/api

Write-Host "🔍 Testing with CORRECT URL Structure (.halopsa.com)..." -ForegroundColor Cyan
Write-Host "====================================================" -ForegroundColor Cyan

# Using the correct domain from documentation
$haloAccount = "panoramicdlsandbox"
$clientId = "0c743b18-d9fd-4bc8-b08d-ca9d22b12d63"
$clientSecret = "055114b9-3997-4bf9-9811-56431776c7af-cf94ffcc-e451-4a5b-8619-f4536e4681dc"

$baseUrl = "https://$haloAccount.halopsa.com"  # Changed from .halopsa.com to .halopsa.com
Write-Host "🌐 Base URL: $baseUrl" -ForegroundColor Yellow
Write-Host "🔑 Client ID: $clientId" -ForegroundColor Yellow
Write-Host "🔐 Client Secret: $($clientSecret.Substring(0,20))..." -ForegroundColor Yellow
Write-Host ""

# Test authentication with correct endpoint
Write-Host "🔐 Testing authentication with correct endpoint..." -ForegroundColor Green

$body = @{
    grant_type = "client_credentials"
    client_id = $clientId
    client_secret = $clientSecret
    scope = "all"
}

try {
    $authUrl = "$baseUrl/auth/token"  # Should be /auth/token, not /auth
    Write-Host "   Auth URL: $authUrl" -ForegroundColor White
    
    $authResponse = Invoke-WebRequest -Uri $authUrl -Method POST -Body $body -ContentType "application/x-www-form-urlencoded" -TimeoutSec 10
    
    Write-Host "   Status: $($authResponse.StatusCode)" -ForegroundColor White
    Write-Host "   Content-Type: $($authResponse.Headers.'Content-Type')" -ForegroundColor White
    Write-Host "   Content Length: $($authResponse.Content.Length)" -ForegroundColor White
    
    if ($authResponse.StatusCode -eq 200) {
        Write-Host "   ✅ SUCCESS! Authentication worked!" -ForegroundColor Green
        
        # Try to parse as JSON
        try {
            $jsonResponse = $authResponse.Content | ConvertFrom-Json
            if ($jsonResponse.access_token) {
                $token = $jsonResponse.access_token
                Write-Host "   🎯 Got access token: $($token.Substring(0,[Math]::Min(20,$token.Length)))..." -ForegroundColor Green
                Write-Host "   Token type: $($jsonResponse.token_type)" -ForegroundColor White
                Write-Host "   Expires in: $($jsonResponse.expires_in) seconds" -ForegroundColor White
                
                # Test the API with this token
                Write-Host ""
                Write-Host "🎯 Testing API endpoints with token..." -ForegroundColor Green
                
                $headers = @{
                    Authorization = "Bearer $token"
                    'Content-Type' = 'application/json'
                }
                
                $apiUrl = "$baseUrl/api/tickets"
                Write-Host "   Testing: $apiUrl" -ForegroundColor White
                
                try {
                    $apiResponse = Invoke-WebRequest -Uri $apiUrl -Method GET -Headers $headers -TimeoutSec 10
                    Write-Host "   ✅ API call successful! Status: $($apiResponse.StatusCode)" -ForegroundColor Green
                    Write-Host "   Response length: $($apiResponse.Content.Length)" -ForegroundColor White
                    Write-Host "   Response preview: $($apiResponse.Content.Substring(0,[Math]::Min(200,$apiResponse.Content.Length)))" -ForegroundColor White
                    Write-Host ""
                    Write-Host "🎉 PERFECT! Everything is working with correct URLs!" -ForegroundColor Green
                    Write-Host "   Your .NET library will now work correctly!" -ForegroundColor Green
                    exit 0
                }
                catch {
                    Write-Host "   ❌ API call failed: $($_.Exception.Message)" -ForegroundColor Red
                    if ($_.Exception.Response.StatusCode -eq 401) {
                        Write-Host "   This might be a token or permissions issue" -ForegroundColor Yellow
                    }
                    elseif ($_.Exception.Response.StatusCode -eq 404) {
                        Write-Host "   API endpoint not found - check the API path" -ForegroundColor Yellow
                    }
                }
            }
        }
        catch {
            Write-Host "   ⚠️  Response is not valid JSON: $($authResponse.Content.Substring(0,[Math]::Min(300,$authResponse.Content.Length)))" -ForegroundColor Yellow
        }
    }
    else {
        Write-Host "   ❌ Auth failed: $($authResponse.StatusCode)" -ForegroundColor Red
        Write-Host "   Response: $($authResponse.Content.Substring(0,[Math]::Min(300,$authResponse.Content.Length)))" -ForegroundColor Red
    }
}
catch {
    Write-Host "   ❌ Exception: $($_.Exception.Message)" -ForegroundColor Red
    if ($_.Exception.Response) {
        Write-Host "   Status Code: $($_.Exception.Response.StatusCode)" -ForegroundColor Red
    }
}

Write-Host ""
Write-Host "📝 Summary:" -ForegroundColor Cyan
Write-Host "   Fixed URL structure:" -ForegroundColor White
Write-Host "   ❌ Old: https://panoramicdlsandbox.halopsa.com" -ForegroundColor Red
Write-Host "   ✅ New: https://panoramicdlsandbox.halopsa.com" -ForegroundColor Green
Write-Host ""
Write-Host "   Authorization Endpoint: $baseUrl/auth" -ForegroundColor White
Write-Host "   Resource Server: $baseUrl/api" -ForegroundColor White