# 登入註冊系統
## 目的
登入系統說起來非常簡單，但要注意的細節非常的多。除了基本的身分驗證外，資訊安全防護上更是不能省略的一塊。
以下是此系統必須注意的幾項目標
* 正確的身分驗證
* 不允許惡意用戶嘗試獲取系統會員資料（例如：Email）。
* 不允許惡意用戶嘗試其他人的帳號密碼。
* 避免DDOS攻擊
* 清楚且不洩漏其他資料的錯誤提示
* 登入token的過期機制(或僅在重要資料和交易前，再次驗證新token)

## 流程
### 登入檢核
檢核後錯誤，錯誤內容以紅框顯示，錯誤訊息置於紅框下。
檢核順序如下：
1. 前端檢查帳號格式是否正確、密碼長度是否符合長度限制。
  －帳號檢核失敗錯誤訊息：
    ＊請確認帳號格式是否正確
  －密碼長度不符合規則：
    ＊請確認帳號密碼是否正確
2. 檢核帳號是否存在 （從快取系統撈或db查詢）
   －錯誤訊息：
    ＊此帳號未註冊，<a>註冊新帳號？</a>－同一裝置嘗試錯誤：
    ＊一天輸入錯誤超過15次，當日不管輸入什麼帳號都顯示錯誤訊息：請確認帳號密碼是否正確
    ＊當日輸入錯誤次數超過15次，於後台異常用戶顯示相關資訊（時間、ip、裝置、嘗試頻率、嘗試紀錄）

3. 檢核帳號密碼是否正確
   －錯誤訊息：
    ＊請確認帳號密碼是否正確
   －嘗試錯誤
    ＊輸入錯誤超過三次，出現檢核碼確認。
    ＊輸入錯誤超過五次，鎖定帳號30分鐘，並寄郵件通知用戶。
    ＊一天輸入錯誤超過二十次，鎖定帳號一天，並於後台異常用戶報表內呈現。
    
### 忘記密碼
檢核後錯誤，以紅框顯示，錯誤訊息置於紅框下。檢核順序如下：
1. 前端檢查帳號格式是否正確
  －帳號檢核失敗錯誤訊息：
    ＊請確認email格式是否正確
2. 檢核帳號是否存在 （從快取系統撈或db查詢）
  －錯誤訊息：
    ＊此帳號未註冊，<a>註冊新帳號？</a>－同一裝置嘗試錯誤：
    ＊一天輸入錯誤超過15次，當日不管輸入什麼帳號都顯示錯誤訊息：請確認帳號密碼是否正確
    ＊當日輸入錯誤次數超過15次，於後台異常用戶顯示相關資訊（時間、ip、裝置、嘗試頻率、嘗試紀錄）

### 註冊
檢核後錯誤，以紅框顯示，錯誤訊息置於紅框下。
註冊欄位
- 帳號(Email)[必填]
  1. 檢查Email格式是否符合標準=>錯誤訊息：請確認email格式是否正確
  2. 檢查是否重複註冊=>錯誤訊息：帳號已註冊<a>登入?</a>
    =>該裝置輸入重複註冊的Email超過15次，不管輸入什麼帳號都顯示錯誤訊息：帳號已註冊<a>登入?</a>
    =>當日輸入錯誤次數超過15次，於後台異常用戶顯示相關資訊（時間、ip、裝置、嘗試頻率、嘗試紀錄）
- 密碼[必填]
  1. 需包含大小寫和數字，8碼以上
  2. 與再次確認密碼不符，密碼、確認密碼皆紅框
    => 錯誤訊息：請確認密碼輸入一致
- 生日
  1. 預設在1990/1/1，年份區間：1911~(今年-3)，例如：1911~2017。
- 手機
  1. 需輸入「國碼」、「手機號碼」。
  2. 檢查手機格式是否符合標準
    =>錯誤訊息：請確認手機格式是否正確
- 姓名
  1. 字數限制30字
- 地址
  1. 需包含「國家」、「縣市」、「行政區」、「詳細地址」

  
