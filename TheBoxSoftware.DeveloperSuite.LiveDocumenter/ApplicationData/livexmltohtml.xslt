<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
  >
  <xsl:output method="html" indent="yes"/>

  <xsl:template match="/livedocument">
    <html>
      <head>
        <title></title>
      </head>
      <body>
        <xsl:apply-templates />
      </body>
    </html>
  </xsl:template>

  <xsl:template match="header1">
    <h1><xsl:apply-templates /></h1>
  </xsl:template>

  <xsl:template match="header2">
    <h1><xsl:apply-templates /></h1>
  </xsl:template>

  <xsl:template match="paragraph">
    <p><xsl:apply-templates /></p>
  </xsl:template>

  <xsl:template match="code" xml:space="preserve">
    <pre><xsl:apply-templates /></pre>
  </xsl:template>

  <xsl:template match="keyword">
    <span class="keyword"><xsl:apply-templates /></span>
  </xsl:template>

  <xsl:template match="run">
    <xsl:apply-templates />
  </xsl:template>
</xsl:stylesheet>
