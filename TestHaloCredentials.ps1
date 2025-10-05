#!/usr/bin/env pwsh

# Simple PowerShell script to test Halo API credentials using correct authentication method
# Run with: pwsh TestHaloCredentials.ps1

Write-Host "🔍 Testing Halo API Credentials (Password Grant)..." -ForegroundColor Cyan
Write-Host "=================================================" -ForegroundColor Cyan

# You'll need to update these with your actual credentials
# Based on the official documentation, we need username/password, not client_id/client_secret
$haloAccount = "panoramicdlsandbox"
$clientId = "8f7c282e-634a-42dc-8c85-23b35b513127"
$username = "your-username-here"  # ⚠️ NEED TO UPDATE THIS
$password = "your-password-here"  # ⚠️ NEED TO UPDATE THIS
$tenant = "Halo"  # For hosted solutions

$baseUrl = "https://$haloAccount.halopsa.com"
Write-Host "🌐 Base URL: $baseUrl" -ForegroundColor Yellow
Write-Host "🔑 Client ID: $clientId" -ForegroundColor Yellow
Write-Host "👤 Username: $username" -ForegroundColor Yellow
Write-Host "🔐 Password: $($password.Substring(0,[Math]::Min(3,$password.Length)))..." -ForegroundColor Yellow
Write-Host "🏢 Tenant: $tenant" -ForegroundColor Yellow
Write-Host ""

# Test 1: Check if the base URL is reachable
Write-Host "📡 Test 1: Checking base URL accessibility..." -ForegroundColor Green
try {
    $pingResponse = Invoke-WebRequest -Uri $baseUrl -Method GET -TimeoutSec 10
    Write-Host "   Status: $($pingResponse.StatusCode)" -ForegroundColor White
    Write-Host "   Content-Type: $($pingResponse.Headers.'Content-Type')" -ForegroundColor White
}
catch {
    Write-Host "   ❌ Failed to reach base URL: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}

# Check if credentials are updated
if ($username -eq "your-username-here" -or $password -eq "your-password-here") {
    Write-Host ""
    Write-Host "⚠️  WARNING: You need to update the username and password in this script!" -ForegroundColor Yellow
    Write-Host "   Based on the official Halo API documentation:" -ForegroundColor Yellow
    Write-Host "   1. You need an actual Halo user account (agent username/password)" -ForegroundColor Yellow
    Write-Host "   2. The authentication uses 'password' grant type, not 'client_credentials'" -ForegroundColor Yellow
    Write-Host "   3. Update the username and password variables at the top of this script" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "   Example user secrets should be:" -ForegroundColor Yellow
    Write-Host '   {' -ForegroundColor Gray
    Write-Host '     "HaloApi": {' -ForegroundColor Gray
    Write-Host '       "HaloAccount": "panoramicdlsandbox",' -ForegroundColor Gray
    Write-Host '       "HaloClientId": "8f7c282e-634a-42dc-8c85-23b35b513127",' -ForegroundColor Gray
    Write-Host '       "HaloUsername": "your-actual-agent-username",' -ForegroundColor Gray
    Write-Host '       "HaloPassword": "your-actual-agent-password",' -ForegroundColor Gray
    Write-Host '       "Tenant": "Halo"' -ForegroundColor Gray
    Write-Host '     }' -ForegroundColor Gray
    Write-Host '   }' -ForegroundColor Gray
    exit 1
}

# Test 2: Try authentication with correct password grant
Write-Host "🔐 Test 2: Testing password-based authentication..." -ForegroundColor Green

$body = @{
    grant_type = "password"
    client_id = $clientId
    username = $username
    password = $password
    scope = "all"
}

try {
    # For hosted solutions, add tenant parameter
    $authUrl = "$baseUrl/auth/token?tenant=$tenant"
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
                
                # Test 3: Try to use the token
                Write-Host ""
                Write-Host "🎯 Test 3: Testing API endpoints with token..." -ForegroundColor Green
                
                $headers = @{
                    Authorization = "Bearer $token"
                    'Content-Type' = 'application/json'
                }
                
                $apiUrl = "$baseUrl/api/tickets"
                Write-Host "   Testing: $apiUrl" -ForegroundColor White
                
                try {
                    $apiResponse = Invoke-WebRequest -Uri $apiUrl -Method GET -Headers $headers -TimeoutSec 10
                    Write-Host "   Status: $($apiResponse.StatusCode)" -ForegroundColor White
                    
                    if ($apiResponse.StatusCode -eq 200) {
                        Write-Host "   ✅ API endpoint works!" -ForegroundColor Green
                        Write-Host "   Response preview: $($apiResponse.Content.Substring(0,[Math]::Min(200,$apiResponse.Content.Length)))" -ForegroundColor White
                        Write-Host ""
                        Write-Host "🎉 SUCCESS! The authentication and API are working correctly!" -ForegroundColor Green
                        Write-Host "   You can now update your user secrets with the correct credentials." -ForegroundColor Green
                        exit 0
                    }
                    else {
                        Write-Host "   ❌ API call failed: $($apiResponse.StatusCode)" -ForegroundColor Red
                    }
                }
                catch {
                    Write-Host "   ❌ API call exception: $($_.Exception.Message)" -ForegroundColor Red
                }
            }
        }
        catch {
            Write-Host "   ⚠️  Response is not JSON: $($authResponse.Content.Substring(0,[Math]::Min(200,$authResponse.Content.Length)))" -ForegroundColor Yellow
        }
    }
    else {
        Write-Host "   ❌ Authentication failed: $($authResponse.Content.Substring(0,[Math]::Min(500,$authResponse.Content.Length)))" -ForegroundColor Red
    }
}
catch {
    Write-Host "   ❌ Exception: $($_.Exception.Message)" -ForegroundColor Red
    if ($_.Exception.Response) {
        $reader = New-Object System.IO.StreamReader($_.Exception.Response.GetResponseStream())
        $responseContent = $reader.ReadToEnd()
        Write-Host "   Response: $($responseContent.Substring(0,[Math]::Min(300,$responseContent.Length)))" -ForegroundColor Red
    }
}

Write-Host ""
Write-Host "💥 Authentication test completed!" -ForegroundColor Red
Write-Host "If authentication failed, this suggests:" -ForegroundColor Red
Write-Host "1. The username/password credentials are incorrect" -ForegroundColor Red
Write-Host "2. The user account doesn't have API permissions" -ForegroundColor Red
Write-Host "3. The tenant parameter is incorrect for hosted solutions" -ForegroundColor Red
Write-Host "4. The client_id doesn't match your application registration" -ForegroundColor Red
exit 1