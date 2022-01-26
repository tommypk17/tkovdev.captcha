<h1>tkovdev.captcha</h1>
<p>Simple, private, & effective captcha package to provide on-the-spot captcha generation and validation to existing and new WebAPIs built on C#.</p>
<h3>Summary</h3>
<hr/>
<p>
    This package can be added to existing WebAPIs to protect forms and data entry from robots.
    Existing solutions are on the market, e.g. Google's reCaptcha system. While those systems are quite
    effective and robust, there is a higher barrier-to-entry for those systems. They also utlize cookies and tracking
    software to work. 
    <br>
    <br>
    This package is robust enough and simple enough to avoid making the privacy tradeoffs that come from
    solutions that use cookies and other tracking measures. This system primarily operates on three fundamental keys.
</p>
<ul>
    <li>Obfuscated graphics</li>
    <li>Hashing and salting</li>
    <li>Timestamps</li>
</ul>
<p>
    The graphics are simple, yet they still provide a challenge for OCR & robots.
    The system uses a secret (salt) and hashing to protect against bruteforce.
    Finally, the use of timestamps inside the hash as well as a validation point ensures the same hash to occur is minimized.
    With simply 6 characters (62 permutations), this equates to 44 million unique values. Adding in timestamps makes them fully unique.
</p>
