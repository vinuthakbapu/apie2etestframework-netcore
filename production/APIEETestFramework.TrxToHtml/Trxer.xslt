<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:t="http://microsoft.com/schemas/VisualStudio/TeamTest/2010"
                version="2.0">

  <xsl:output method="html" indent="yes"/>
  <xsl:key name="TestMethods" match="t:TestMethod" use="@className"/>

  <xsl:template match="/" >
    <xsl:text disable-output-escaping='yes'>&lt;!DOCTYPE html></xsl:text>
    <html>
      <head>
        <meta charset="utf-8"/>
        <link rel="stylesheet" type="text/css" href="Trxer.css"/>
        <link rel="stylesheet" type="text/css" href="TrxerTable.css"/>
        <script language="javascript" type="text/javascript" src="functions.js"></script>
        <title>
          <xsl:value-of select="/t:TestRun/@name"/>
        </title>
      </head>
      <body>
        <div id="divToRefresh" class="wrapOverall">
          <div id="floatingImageBackground" class="floatingImageBackground" style="visibility: hidden;">
            <div class="floatingImageCloseButton" onclick="hide('floatingImageBackground');"></div>
            <img src="" id="floatingImage" onclick="OpenInNewWindow();"/>
            <div style="margin-left: 200px;">Click image to view in new browser tab</div>
          </div>
          <br />
          <xsl:variable name="testRunOutcome" select="t:TestRun/t:ResultSummary/@outcome"/>

          <div class="StatusBar statusBar{$testRunOutcome}">
            <div class="statusBar{$testRunOutcome}Inner">
              <center>
                <h1 class="hWhite">
                  <div class="titleCenterd">
                    <xsl:value-of select="/t:TestRun/@name"/>
                  </div>
                </h1>
              </center>
            </div>
          </div>
          <div class="SummaryDiv">
            <table id="TotalTestsTable">
              <caption>Results Summary</caption>
              <thead>
                <tr class="odd">
                  <th scope="col" abbr="Status">
                    Pie View
                  </th>
                </tr>
              </thead>
              <tbody>
                <tr>
                  <td>
                    <div id="dataViewer"></div>
                  </td>
                </tr>
                <tr id="DownloadSection">
                  <td>
                    <a href="#" class="button" id="btn-download" download="{/t:TestRun/@name}StatusesPie.png">Save graph</a>
                  </td>
                </tr>
              </tbody>
            </table>
            <table class="DetailsTable StatusesTable">
              <caption>Tests Statuses</caption>
              <tbody>
                <tr class="odd">
                  <th class="column1 statusCount">Total</th>
                  <td class="statusCount">
                    <xsl:value-of select="/t:TestRun/t:ResultSummary/t:Counters/@total" />
                  </td>
                </tr>
                <tr>
                  <th scope="row" class="column1 statusCount">Executed</th>
                  <td class="statusCount">
                    <xsl:value-of select="/t:TestRun/t:ResultSummary/t:Counters/@executed" />
                  </td>
                </tr>
                <tr>
                  <th scope="row" class="column1 statusCount">Passed</th>
                  <td class="statusCount">
                    <xsl:value-of select="/t:TestRun/t:ResultSummary/t:Counters/@passed" />
                  </td>
                </tr>
                <tr>
                  <th scope="row" class="column1 statusCount">Failed</th>
                  <td class="statusCount">
                    <xsl:value-of select="/t:TestRun/t:ResultSummary/t:Counters/@failed" />
                  </td>
                </tr>
                <tr>
                  <th scope="row" class="column1 statusCount">Inconclusive</th>
                  <td class="statusCount">
                    <xsl:value-of select="/t:TestRun/t:ResultSummary/t:Counters/@inconclusive" />
                  </td>
                </tr>
                <tr>
                  <th scope="row" class="column1 statusCount">Error</th>
                  <td class="statusCount">
                    <xsl:value-of select="/t:TestRun/t:ResultSummary/t:Counters/@error" />
                  </td>
                </tr>
                <tr>
                  <th scope="row" class="column1 statusCount">Warning</th>
                  <td class="statusCount">
                    <xsl:value-of select="/t:TestRun/t:ResultSummary/t:Counters/@warning" />
                  </td>
                </tr>
                <tr>
                  <th scope="row" class="column1 statusCount">Timeout</th>
                  <td class="statusCount">
                    <xsl:value-of select="/t:TestRun/t:ResultSummary/t:Counters/@timeout" />
                  </td>
                </tr>
              </tbody>
            </table>
            <table class="SummaryTable">
              <caption>Run Time Summary</caption>
              <tbody>
                <xsl:for-each select="/t:TestRun/t:Times">
                  <tr class="odd">
                    <th class="column1">Start Time</th>
                    <td>
                      <!--<xsl:value-of select="GetShortDateTime(@start)" />-->
                      <xsl:value-of select="@start"/>
                    </td>
                  </tr>
                  <tr>
                    <th class="column1">End Time</th>
                    <td>
                      <!--<xsl:value-of select="GetShortDateTime(@finish)" />-->
                      <xsl:value-of select="@finish"/>
                    </td>
                  </tr>                  
                </xsl:for-each>
              </tbody>
            </table>
          </div>
          <xsl:variable name="testsSet" select="//t:TestRun/t:Results/t:UnitTestResult" />
          <table>
            <tbody>
              <caption>
              </caption>                         
              <tr>
                <td>
                  <a class="FiterResults" onclick='ReplaceClass("//tr[contains(@id,\"testRow\")]", "hiddenRow", "visibleRow");
                                                   ClearValue("//input[contains(@id,\"filter\")]");'>
                    <div class="MoreButtonText">Show All Tests</div>
                  </a>
                </td>
                <td>                  
                  <a class="FiterResults" onclick='UncheckAll("//tr[contains(@id,\"testRow\") and contains(@class,\"selected\") and contains(@class,\"visibleRow\")]//input[@type=\"checkbox\"]");
                                                   ReplaceClass("//tr[contains(@id,\"testRow\") and contains(@class,\"selected\")]", "visibleRow", "hiddenRow");
                                                   ReplaceClass("//tr[contains(@id,\"TestsContainer\")][./preceding-sibling::tr[1][contains(@id,\"testRow\") and contains(@class,\"selected\")]]", "visibleRow", "hiddenRow");
                                                   ClearValue("//input[contains(@id,\"filter\")]");
                                                   ReplaceClass("//tr[contains(@id,\"testRow\") and contains(@class,\"selected\")]", " selected", "");
                                                   '>
                    <div class="MoreButtonText">Hide and Uncheck Selected Tests</div>
                  </a>             
                  <a class="FiterResults" onclick='ReplaceClass("//tr[contains(@id,\"testRow\")][contains(@class,\"testPassed\")]", "visibleRow", "hiddenRow");
                                                   ReplaceClass("//tr[contains(@id,\"TestsContainer\")][./preceding-sibling::tr[1][contains(@id,\"testRow\") and contains(@class,\"testPassed\")]]", "visibleRow", "hiddenRow");
                                                   ClearValue("//input[contains(@id,\"filter\")]");'>
                    <div class="MoreButtonText">Hide Passed Tests</div>
                  </a>
                  <a class="ShowRules" onclick='ReplaceClass("//tr[contains(@id,\"infoRow\")]", "hiddenRow", "visibleRow");cursor:pointer;'>click here for help</a>
                </td>
              </tr>
              <tr id="infoRow" class="hiddenRow">
                <td/>
                <td>
                  <p style="text-align: left; padding: 10px 10px 10px 10px;">
                    1. Don't use IE. Reports work in Chrome and Firefox browser only. Chrome is preferrable.
                    <br/>
                    2. You can click TestMethod name to view logs.
                    <br/>
                    3. When logs are expanded, additionally you can click image icon to view screenshot, or "Show Stacktrace link" to view stacktrace, it they are present (usually stacktrace and screenshot are displayed in the failed tests only).
                    <br/>
                    4. Button "Hide Passed Tests" does not uncheck passed rows, it just hides them.
                    <br/>
                    5. You can click Column titles marked by "↕" sign to sort. Sorting can be applied only by 1 column. Sorting is applied for visible rows only. For ~120 rows sorting may take about 5 seconds, during which page is unresponsive.
                    <br/>
                    6. You can type search keyword in multiple search-boxes and the click "filter" button. Results, that correspond to all the specified search keywords, will be displayed only.                    
                    <br/>
                    7. Checkbox Check / Uncheck All will check and uncheck only visible rows. If you wish to check/uncheck invisible rows, click "Show All Tests" before.                    
                  </p>
                  <a class="ShowRules" onclick='ReplaceClass("//tr[contains(@id,\"infoRow\")]", "visibleRow", "hiddenRow");cursor:pointer;'>hide</a>
                </td>
              </tr>
            </tbody>
          </table>
          <table id="ReportsTable">
            <thead>
              <tr class="odd">
                <th scope="col" abbr="Test">
                  <input type="checkbox" class="checkboxAll" onclick='CheckUncheckAll("//input[@class=\"checkboxAll\"]","//tr[contains(@id,\"testRow\")][contains(@class,\"visibleRow\")]//input[@class=\"testCheckbox\"]");
                  CheckUncheck("//input[@class=\"checkboxAll\"]","//tr[contains(@id,\"testRow\")][contains(@class,\"visibleRow\")]")' />
                </th>
                <th id="columnTestMethod" scope="col" abbr="Test">
                  <div onclick="SortRows('TestMethod')" style="cursor: pointer;">Test Method↕</div>
                </th>
                <th id="columnTestClass" scope="col">
                  <div onclick="SortRows('TestClass')" style="cursor: pointer;">Test Class↕</div>
                </th>               
                <th id="columnErrorMessage" scope="col">
                  <div onclick="SortRows('ErrorMessage')" style="cursor: pointer;">Error Message↕</div>
                </th>
                <th id="columnStartTime" scope="col">
                  <div onclick="SortRows('StartTime')" style="cursor: pointer;">Start Time↕</div>
                </th>
                <th id="columnDuration" scope="col">
                  <div onclick="SortRows('Duration')" style="cursor: pointer;">Duration↕</div>
                </th>
              </tr>
            </thead>
            <tbody>
              <tr>
                <td style="padding:3px;">
                  <button onclick='ReplaceClass("//tr[contains(@id,\"testRow\")]", "visibleRow", "hiddenRow");
                                    ReplaceClass("//tr[contains(@id,\"TestsContainer\")]", "visibleRow", "hiddenRow");
                                    ReplaceText("//div[@class=\"OpenMoreButton\"]/div[contains(@class,\"MoreButtonText\")]", "", "");
                                    FilterRows("TestMethod", "TestClass", "ErrorMessage", "StartTime", "Duration");'
                          style="font-size: 17px;float: right;cursor: pointer;width: 100%;padding: 0px 0px 0px 0px;">filter</button>
                </td>
                <td>
                  <input id="filterTestMethod" style="width: 100%;"/>
                </td>
                <td>
                  <input id="filterTestClass" style="width: 100%;"/>
                </td>                
                <td>
                  <input id="filterErrorMessage" style="width: 100%;"/>
                </td>
                <td>
                  <input id="filterStartTime" style="width: 100%;"/>
                </td>
                <td>
                  <input id="filterDuration" style="width: 100%;"/>
                </td>
              </tr>


              <xsl:for-each select="$testsSet">
                <xsl:variable name="testId" select="@testId" />

                <xsl:variable name="rowVisibility">
                  <xsl:choose>
                    <xsl:when test="@outcome = 'Failed'">visibleRow</xsl:when>
                    <xsl:otherwise>hiddenRow</xsl:otherwise>
                  </xsl:choose>
                </xsl:variable>

                <xsl:variable name="rowId">
                  testRow <xsl:value-of select="generate-id(@testId)"/>
                </xsl:variable>
                <tr id="{$rowId}" class="{$rowVisibility} test{@outcome}">
                  <xsl:variable name="outputLog" select="t:Output/t:StdOut"/>
                  <th class="column1{@outcome}">
                    <input type="checkbox" class="testCheckbox" onclick='CheckUncheck("//tr[@id=\"{$rowId}\"]//input","//tr[@id=\"{$rowId}\"]")' />
                  </th>
                  <td id="rowTestMethod" scope="row" class="column1{@outcome}">
                    <div class="OpenMoreButton" onclick="ShowHide('{generate-id(@testId)}TestsContainer','{generate-id(@testId)}Button','{@testName}','{@testName}');">
                      <div class="MoreButtonText testName{@outcome}" id="{generate-id(@testId)}Button">
                        <xsl:value-of select="@testName" />
                      </div>
                    </div>
                  </td>
                  <td id="rowTestClass" class="LongText">
                    <span>
                      <xsl:value-of select="//t:TestRun/t:TestDefinitions/t:UnitTest[@id=$testId]/t:TestMethod/@className" />
                    </span>
                  </td>                 
                  <td id="rowErrorMessage" class="LongText">
                    <font size="2">
                      <xsl:value-of select="t:Output/t:ErrorInfo/t:Message/text()" />                      
                    </font>
                  </td>
                  <td id="rowStartTime" class="LongText">
                    <span>
                      <!--<xsl:value-of select="GetShortDateTime(@startTime)"/>-->
                      <xsl:value-of select="@startTime"/>
                    </span>
                  </td>
                  <td id="rowDuration" class="LongText">
                    <span>
                      <!--<xsl:value-of select="ParseDuration(@duration)" />-->
                      <xsl:value-of select="@duration" />
                    </span>
                  </td>
                </tr>
                <tr id="{generate-id(@testId)}TestsContainer" class="hiddenRow">
                  <td colspan="100">
                    <div id="exceptionArrow">↳</div>
                    <table>
                      <thead>
                        <tr class="odd">
                          <th scope="col" class="TestsTable" abbr="Status">Status</th>
                          <th scope="col" class="TestsTable" abbr="Test">Screenshot</th>
                          <th scope="col" class="TestsTable" abbr="Message" style="width:100%">Log</th>
                          <th scope="col" class="TestsTable" abbr="Test">Machine</th>
                        </tr>
                      </thead>
                      <tbody>
                        <xsl:for-each select="//t:TestRun/t:Results/t:UnitTestResult[@testId=$testId]">
                          <xsl:call-template name="tDetails">
                            <xsl:with-param name="testId" select="@testId" />
                            <xsl:with-param name="description">All</xsl:with-param>
                          </xsl:call-template>
                        </xsl:for-each>
                      </tbody>
                    </table>
                  </td>
                </tr>
                <script>
                  CalculateTestsStatuses('<xsl:value-of select="generate-id(@testId)"/>TestsContainer','<xsl:value-of select="generate-id(@testId)"/>canvas');
                </script>
              </xsl:for-each>
            </tbody>
          </table>
          <table>
            <caption>Five most slowest tests</caption>
            <thead>
              <tr class="odd">
                <th scope="col">Time</th>
                <th scope="col" abbr="Status">Status</th>
                <th scope="col" abbr="Test">Test</th>                
                <th scope="col" abbr="Message">Duration</th>
              </tr>
            </thead>
            <tbody>
              <xsl:for-each select="/t:TestRun/t:Results/t:UnitTestResult">
                <xsl:sort select="@duration" order="descending"/>
                <xsl:if test="position() &gt;= 1 and position() &lt;=5">
                  <xsl:variable name="testId" select="@testId" />
                  <tr>
                    <th scope="row" class="column1">
                      <xsl:value-of select="@startTime"/>
                    </th>
                    <xsl:call-template name="tStatus">
                      <xsl:with-param name="testId" select="@testId" />
                    </xsl:call-template>
                    <td class="Function slowest">
                      <xsl:value-of select="@testName"/>
                    </td>                    
                    <td class="Message slowest">
                      <xsl:value-of select="@duration" />
                    </td>
                  </tr>
                </xsl:if>
              </xsl:for-each>              
            </tbody>
          </table>
        </div>
      </body>
      <script>
        CalculateTotalPrecents();
      </script>
    </html>
  </xsl:template>


  <xsl:template name="tStatus">
    <xsl:param name="testId" />
    <xsl:for-each select="/t:TestRun/t:Results/t:UnitTestResult[@testId=$testId]">
      <xsl:choose>
        <xsl:when test="@outcome='Passed'">
          <td class="passed">PASSED</td>
        </xsl:when>
        <xsl:when test="@outcome='Failed'">
          <td class="failed">FAILED</td>
        </xsl:when>
        <xsl:when test="@outcome='Inconclusive'">
          <td class="warn">Inconclusive</td>
        </xsl:when>
        <xsl:when test="@outcome='Timeout'">
          <td class="failed">Timeout</td>
        </xsl:when>
        <xsl:when test="@outcome='Error'">
          <td class="failed">Error</td>
        </xsl:when>
        <xsl:when test="@outcome='Warn'">
          <td class="warn">Warn</td>
        </xsl:when>
        <xsl:otherwise>
          <td class="info">OTHER</td>
        </xsl:otherwise>
      </xsl:choose>
    </xsl:for-each>
  </xsl:template>


  <xsl:template name="tDetails">
    <xsl:param name="testId" />
    <xsl:param name="description" />
    <xsl:for-each select="/t:TestRun/t:Results/t:UnitTestResult[@testId=$testId]">
      <tr class="Test">
        <xsl:call-template name="tStatus">
          <xsl:with-param name="testId" select="$testId" />
        </xsl:call-template>
        <th scope="row" class="column1">
          <xsl:call-template name="imageExtractor">
            <xsl:with-param name="testId" select="$testId" />
          </xsl:call-template>
        </th>
        <td class="Messages">
          <xsl:call-template name="debugInfo">
            <xsl:with-param name="testId" select="$testId" />
            <xsl:with-param name="description" select="$description" />
          </xsl:call-template>
        </td>
        <td>
          <xsl:value-of select="@computerName"/>
        </td>
      </tr>
      <tr id="{generate-id($testId)}{$description}Stacktrace" class="hiddenRow">
        <!--Outer-->
        <td colspan="6">
          <div id="exceptionArrow">↳</div>
          <table>
            <!--Inner-->
            <tbody>
              <tr class="visibleRow">
                <td class="ex" colspan="100">
                  <pre style="font-size: 12px;">
                    <xsl:value-of select="/t:TestRun/t:Results/t:UnitTestResult[@testId=$testId]/t:Output/t:ErrorInfo/t:StackTrace" />
                  </pre>
                </td>
              </tr>
            </tbody>
          </table>
        </td>
      </tr>
    </xsl:for-each>
  </xsl:template>

  <xsl:template name="imageExtractor">
    <xsl:param name="testId" />
    <xsl:for-each select="/t:TestRun/t:Results/t:UnitTestResult[@testId=$testId]/t:Output">

      <xsl:variable name="screenshotLink" select="t:StdOut/t:test/t:screenshot"/>
      <xsl:choose>
        <xsl:when test="$screenshotLink">
          <div class="atachmentImage" onclick="show('floatingImageBackground');updateFloatingImage('{$screenshotLink/@href}');"></div>
        </xsl:when>
      </xsl:choose>

    </xsl:for-each>
  </xsl:template>







  <xsl:template name="debugInfo">
    <xsl:param name="testId" />
    <xsl:param name="description" />
    <xsl:for-each select="/t:TestRun/t:Results/t:UnitTestResult[@testId=$testId]/t:Output">

      <xsl:variable name="MessageErrorStacktrace" select="t:ErrorInfo/t:StackTrace"/>
      <xsl:variable name="StdOut" select="t:StdOut"/>

      <xsl:if test="$StdOut or $MessageErrorStacktrace">
        <textarea style="width: 100%;height: 300px; resize: none;" >
          <xsl:value-of select="$StdOut/t:test"/>
        </textarea>
        <xsl:if test="$StdOut">
          <br/>
        </xsl:if>
      </xsl:if>
      <xsl:value-of select="t:StdErr" />
      <xsl:variable name="StdErr" select="t:StdErr"/>
      <xsl:if test="$StdErr">
        <xsl:value-of select="$StdErr"/>
        <br/>
      </xsl:if>
      <xsl:variable name="MessageErrorInfo" select="t:ErrorInfo/t:Message"/>
      <xsl:if test="$MessageErrorInfo">
        <font style='color: #880B0B; background-color: #F3E0E0;' size="2">
          <b>
            <xsl:value-of select="$MessageErrorInfo"/>
          </b>
        </font>
        <br/>
      </xsl:if>
      <xsl:if test="$MessageErrorStacktrace">
        <a style="float:left;font-weight: bold;color: #100cff;font-size: 20px" id="{generate-id($testId)}{$description}StacktraceToggle" href="javascript:ShowHide('{generate-id($testId)}{$description}Stacktrace','{generate-id($testId)}{$description}StacktraceToggle','Show Stacktrace','Hide Stacktrace');">Show Stacktrace</a>
      </xsl:if>

    </xsl:for-each>
  </xsl:template>

</xsl:stylesheet>

