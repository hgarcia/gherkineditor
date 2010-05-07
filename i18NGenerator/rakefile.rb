namespace :generate do
  desc "Generating Gherkin editor syntax filefor all i18n supported languages in Gherkin"

	task :test do
		require 'gherkin/i18n'
		require 'erb'		
		puts "howdy rake"
		xmlfile = ERB.new(IO.read(File.dirname(__FILE__) + '/customHighlighting.xshd.erb'), nil, '-')
		File.open('CustomHighlighting.xshd', 'wb') do |io|
			keywords = Gherkin::I18n.keyword_regexp(:feature,:background)
			scenarios = Gherkin::I18n.keyword_regexp(:scenario,:scenario_outline)
			steps = Gherkin::I18n.keyword_regexp(:step,:examples)
			
			io.write(xmlfile.result(binding))
		end
	end

end
