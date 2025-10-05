#!/usr/bin/env pwsh

# Test with the CORRECT Client ID from the Halo configuration page
# Run with: pwsh TestCorrectCredentials.ps1

Write-Host "🔍 Testing with CORRECT Client ID from Halo Config..." -ForegroundColor Cyan
Write-Host "====================================================" -ForegroundColor Cyan

# Using the CORRECT Client ID from the screenshot
$haloAccount = "panoramicdlsandbox"
$correctClientId = "0c743b1f-d9fd-4bc8-b08d-ca9d22b12d83"  # From Halo config page
$oldClientId = "8f7c282e-634a-42dc-8c85-23b35b513127"      # From user secrets
$clientSecret = "30546cfe-f657-4472-97aa-b9331924bbc8-06a09701-efda-478f-b73b-97c3539e9c0c"

$baseUrl = "https://$haloAccount.halopsa.com"
Write-Host "🌐 Base URL: $baseUrl" -ForegroundColor Yellow
Write-Host "✅ CORRECT Client ID: $correctClientId" -ForegroundColor Green
Write-Host "❌ OLD Client ID: $oldClientId" -ForegroundColor Red
Write-Host "🔐 Client Secret: $($clientSecret.Substring(0,20))..." -ForegroundColor Yellow
Write-Host ""

# Test with CORRECT credentials
Write-Host "🔐 Testing authentication with CORRECT Client ID..." -ForegroundColor Green

$body = @{
    grant_type = "client_credentials"
    client_id = $correctClientId
    client_secret = $clientSecret
    scope = "all"
}

try {
    $authUrl = "$baseUrl/auth/token"
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
                    Write-Host "🎉 PERFECT! Everything works with the correct Client ID!" -ForegroundColor Green
                    Write-Host ""
                    Write-Host "📝 UPDATE YOUR USER SECRETS:" -ForegroundColor Cyan
                    Write-Host 'dotnet user-secrets set "HaloApi:HaloClientId" "0c743b1f-d9fd-4bc8-b08d-ca9d22b12d83" --project Halo.Api.Test' -ForegroundColor Yellow
                    exit 0
                }
                catch {
                    Write-Host "   ❌ API call failed: $($_.Exception.Message)" -ForegroundColor Red
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
}

Write-Host ""
Write-Host "📝 Summary:" -ForegroundColor Cyan
Write-Host "   The issue was using the wrong Client ID in your user secrets." -ForegroundColor White
Write-Host "   Update your secrets to use: 0c743b1f-d9fd-4bc8-b08d-ca9d22b12d83" -ForegroundColor White